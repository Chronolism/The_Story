using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Security.Cryptography;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


[Serializable] public class UnityEventString : UnityEvent<String> { }

public class Utils
{
    // ��������float��int���������֧�ָ���
    public static long Clamp(long value, long min, long max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public static bool AnyKeyUp(KeyCode[] keys)
    {
        // ����Linq.Any����Ϊ��Ӱ��GC����
        foreach (KeyCode key in keys)
            if (Input.GetKeyUp(key))
                return true;
        return false;
    }

    public static bool AnyKeyDown(KeyCode[] keys)
    {
        // ����Linq.Any����Ϊ��Ӱ��GC����
        foreach (KeyCode key in keys)
            if (Input.GetKeyDown(key))
                return true;
        return false;
    }

    public static bool AnyKeyPressed(KeyCode[] keys)
    {
        // ����Linq.Any����Ϊ��Ӱ��GC����
        foreach (KeyCode key in keys)
            if (Input.GetKey(key))
                return true;
        return false;
    }

    // 2D point in screen
    public static bool IsPointInScreen(Vector2 point)
    {
        return 0 <= point.x && point.x <= Screen.width &&
               0 <= point.y && point.y <= Screen.height;
    }

    // �������Ƿ�λ��UI��OnGUIԪ����
    public static bool IsCursorOverUserInterface()
    {
        // IsPointerOverGameObject ���r left mouse (default)
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        // IsPointerOverGameObject ��� touches
        for (int i = 0; i < Input.touchCount; ++i)
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                return true;

        // OnGUI check
        return GUIUtility.hotControl != 0;
    }

    // ��������ռ��б߽�뾶�ĸ�������
    // -> collider.radius  local scale
    // -> collider.bounds  world scale
    // -> ʹ��x+y��չƽ��ֵ��ȷ�� (for capsules, x==y extends)
    // -> ʹ�á�extends�������ǡ�size������Ϊextends�ǰ뾶��
    //    ���仰˵��������Ǵ��ұ���������ֻ��ͣ�ڵİ뾶�ǰ뾶��һ�룬���ǰ뾶��������
    public static float BoundsRadius(Bounds bounds) =>
        (bounds.extents.x + bounds.extents.z) / 2;

    // �����պϵ�֮��ľ���
    // Vector3.Distance(a.transform.position, b.transform.position):
    //    _____        _____
    //   |     |      |     |
    //   |  x==|======|==x  |
    //   |_____|      |_____|
    //
    //
    // Utils.ClosestDistance(a.collider, b.collider):
    //    _____        _____
    //   |     |      |     |
    //   |     |x====x|     |
    //   |_____|      |_____|
    public static float ClosestDistance(Entity a, Entity b)
    {
        float distance = Vector3.Distance(a.transform.position, b.transform.position);

        // ������ײ��İ뾶
        float radiusA = BoundsRadius(a.GetComponent<Collider>().bounds);
        float radiusB = BoundsRadius(b.GetComponent<Collider>().bounds);

        // ��ȥ�����뾶
        float distanceInside = distance - radiusA - radiusB;

        // ���ؾ��롣�����С��0�������ڱ˴��ڲ�����ô����0
        return Mathf.Max(distanceInside, 0);
    }

    // ��ʵ�����ײ������һ����������
    public static Vector3 ClosestPoint(Entity entity, Vector3 point)
    {
        float radius = BoundsRadius(entity.GetComponent<Collider>().bounds);

        Vector3 direction = entity.transform.position - point;
        //Debug.DrawLine(point, point + direction, Color.red, 1, false);

        // ��direction length��ȥ�뾶
        Vector3 directionSubtracted = Vector3.ClampMagnitude(direction, direction.magnitude - radius);

        // return the point
        //Debug.DrawLine(point, point + directionSubtracted, Color.green, 1, false);
        return point + directionSubtracted;
    }


    static Dictionary<Transform, int> castBackups = new Dictionary<Transform, int>();

    // ��������ʱ���й���Ͷ�䣨���Ƚ�������Ϊ�����Թ���Ͷ�䡱��
    public static bool RaycastWithout(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance, GameObject ignore, int layerMask = Physics.DefaultRaycastLayers)
    {
        // remember layers
        castBackups.Clear();

        // ȫ������Ϊ���Թ���Ͷ��
        foreach (Transform tf in ignore.GetComponentsInChildren<Transform>(true))
        {
            castBackups[tf] = tf.gameObject.layer;
            tf.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        // raycast
        bool result = Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);

        // restore layers
        foreach (KeyValuePair<Transform, int> kvp in castBackups)
            kvp.Key.gameObject.layer = kvp.Value;

        return result;
    }

    // ������������Ⱦ���ķ�װ�߽�
    public static Bounds CalculateBoundsForAllRenderers(GameObject go)
    {
        Bounds bounds = new Bounds();
        bool initialized = false;
        foreach (Renderer rend in go.GetComponentsInChildren<Renderer>())
        {
            // initialize or encapsulate
            if (!initialized)
            {
                bounds = rend.bounds;
                initialized = true;
            }
            else bounds.Encapsulate(rend.bounds);
        }
        return bounds;
    }

    // �� "from" ��������ı任
    public static Transform GetNearestTransform(List<Transform> transforms, Vector3 from)
    {
        Transform nearest = null;
        foreach (Transform tf in transforms)
        {
            if (nearest == null ||
                Vector3.Distance(tf.position, from) < Vector3.Distance(nearest.position, from))
                nearest = tf;
        }
        return nearest;
    }

    // Ư���Ĵ�ӡ����Сʱ�����ӣ��루.����/100����
    public static string PrettySeconds(float seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        string res = "";
        if (t.Days > 0) res += t.Days + "d";
        if (t.Hours > 0) res += " " + t.Hours + "h";
        if (t.Minutes > 0) res += " " + t.Minutes + "m";
        // 0.5s, 1.5s etc. if any milliseconds. 1s, 2s etc. if any seconds
        if (t.Milliseconds > 0) res += " " + t.Seconds + "." + (t.Milliseconds / 100) + "s";
        else if (t.Seconds > 0) res += " " + t.Seconds + "s";
        // ����ַ�����Ȼ�ǿյģ���Ϊֵ�ǡ�0������ô����
        // ���������������Ƿ��ؿ��ַ���
        return res != "" ? res : "0s";
    }

    // ����ƽ̨֮��һ�µ�Ӳ������
    // ����.GetAxis���������֡�����
    // ����.GetAxisRaw���������֡���
    // ����������ֵ���Ƕ�����0.01��WebGL�ϵ�0.5����
    // ������WebGL�����Ź���ȡ�
    // ͨ��GetAxisRawӦ�÷���-1,0,1�������ڹ����򲻷���
    public static float GetAxisRawScrollUniversal()
    {
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll < 0) return -1;
        if (scroll > 0) return 1;
        return 0;
    }

    // two finger pinch detection
    // source: https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
    public static float GetPinch()
    {
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            return touchDeltaMag - prevTouchDeltaMag;
        }
        return 0;
    }

    // ͨ�ñ佹��������,���������ָ����
    public static float GetZoomUniversal()
    {
        if (Input.mousePresent)
            return GetAxisRawScrollUniversal();
        else if (Input.touchSupported)
            return GetPinch();
        return 0;
    }

    // �����ַ��������һ����д����
    //   EquipmentWeaponBow => Bow
    //   EquipmentShield => Shield
    static Regex lastNountRegEx = new Regex(@"([A-Z][a-z]*)"); // cache to avoid allocations. this is used a lot.
    public static string ParseLastNoun(string text)
    {
        MatchCollection matches = lastNountRegEx.Matches(text);
        return matches.Count > 0 ? matches[matches.Count - 1].Value : "";
    }

    // NIST�Ƽ���PBKDF2��ϣ
    // http://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-132.pdf
    // salt should be at least 128 bits = 16 bytes
    public static string PBKDF2Hash(string text, string salt)
    {
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(text, saltBytes, 10000);
        byte[] hash = pbkdf2.GetBytes(20);
        return BitConverter.ToString(hash).Replace("-", string.Empty);
    }

    // ���÷��䣬ͨ��ǰ׺���ö������
    // -> ���������Ա��㹻��ؽ��и��µ���
    static Dictionary<KeyValuePair<Type, string>, MethodInfo[]> lookup = new Dictionary<KeyValuePair<Type, string>, MethodInfo[]>();
    public static MethodInfo[] GetMethodsByPrefix(Type type, string methodPrefix)
    {
        KeyValuePair<Type, string> key = new KeyValuePair<Type, string>(type, methodPrefix);
        if (!lookup.ContainsKey(key))
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                                       .Where(m => m.Name.StartsWith(methodPrefix))
                                       .ToArray();
            lookup[key] = methods;
        }
        return lookup[key];
    }

    public static void InvokeMany(Type type, object onObject, string methodPrefix, params object[] args)
    {
        foreach (MethodInfo method in GetMethodsByPrefix(type, methodPrefix))
            method.Invoke(onObject, args);
    }

    // clamp a rotation around x axis
    public static Quaternion ClampRotationAroundXAxis(Quaternion q, float min, float max)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, min, max);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    //2dLookAt
    public static Quaternion LookAt2D(Vector3 start , Vector3 end )
    {
        return Quaternion.AngleAxis((Vector3.Cross(start.normalized, end.normalized).z > 0 ? 1 : -1) * Mathf.Acos(Vector3.Dot(start.normalized, end.normalized)) * Mathf.Rad2Deg, new Vector3(0, 0, 1));
    }
}