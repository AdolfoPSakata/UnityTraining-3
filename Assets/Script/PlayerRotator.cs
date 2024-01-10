using UnityEngine.Events;

public class PlayerRotator : Rotator
{
    public UnityAction<float> onRotate;
    public UnityAction<bool> onPressButton;

    private void Start()
    {
        onRotate += StartRotation;
        onPressButton += ChangePressedStatus;
    }
}
