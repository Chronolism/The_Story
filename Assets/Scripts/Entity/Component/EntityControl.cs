using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EntityControl : EntityComponent
{
    int oldInput;
    int newInput;

    public void PlayerInput()
    {
        newInput = 0;

        if (IEnableInput.GetKey(E_PlayKeys.W)) newInput += (int)E_PlayKeys.W;
        if (IEnableInput.GetKey(E_PlayKeys.S)) newInput += (int)E_PlayKeys.S;
        if (IEnableInput.GetKey(E_PlayKeys.D)) newInput += (int)E_PlayKeys.D;
        if (IEnableInput.GetKey(E_PlayKeys.A)) newInput += (int)E_PlayKeys.A;
        if (IEnableInput.GetKey(E_PlayKeys.E)) newInput += (int)E_PlayKeys.E;
        if (IEnableInput.GetKey(E_PlayKeys.Q)) newInput += (int)E_PlayKeys.Q;

        if (newInput == oldInput) return;
        oldInput = newInput;
        PlayerChangeInput(oldInput);
    }

    [Command(requiresAuthority = false)]
    public void PlayerChangeInput(int input)
    {

        entity.inputY = ((input & (int)E_PlayKeys.W) > 0 ? 1 : 0);
        entity.inputY += -((input & (int)E_PlayKeys.S) > 0 ? 1 : 0);
        entity.inputX = ((input & (int)E_PlayKeys.D) > 0 ? 1 : 0);
        entity.inputX += -((input & (int)E_PlayKeys.A) > 0 ? 1 : 0);
        entity.fire2 = ((input & (int)E_PlayKeys.E) > 0 ? 1 : 0);
        entity.fire1 = ((input & (int)E_PlayKeys.Q) > 0 ? 1 : 0);
    }
}
