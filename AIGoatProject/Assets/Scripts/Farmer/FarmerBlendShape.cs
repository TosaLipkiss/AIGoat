using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerBlendShape : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;
    int blendShapeCount;
    float blendBlink = 0f;
    float blendIdle = 0f;
    float blendSpeed = 3f;
    float blinkTimer = 0f;
    int randomBlinkTimerLimit = 1;
    bool blinkedFInished = false;

    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = skinnedMeshRenderer.sharedMesh;

        blendShapeCount = skinnedMesh.blendShapeCount;
    }

    void Update()
    {
        blinkTimer += Time.deltaTime;
    }

    public void BlinkBlend()
    {
        if (blinkTimer > randomBlinkTimerLimit)
        {
            if (blinkedFInished == false && blendBlink < 100f)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, blendBlink);
                blendBlink += blendSpeed;
            }
            else
            {
                blinkedFInished = true;
                blinkTimer = 0f;
            }
        }

        if (blinkedFInished == true && blendBlink > 0f)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, blendBlink);
            blendBlink -= blendSpeed;

            if (blendBlink <= 0f)
            {
                blinkedFInished = false;
                randomBlinkTimerLimit = Random.Range(1, 7);
                blendSpeed = Random.Range(1f, 4f);
            }
        }
    }

    public void BlendCloseEyes()
    {
        if (blendBlink > 0f)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, blendBlink);
            blendBlink -= blendSpeed;
        }
    }

    public void BlendOpenEyes()
    {
        if (blendBlink < 100f)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, blendBlink);
            blendBlink += blendSpeed;
        }
    }

    public void BlendAngry()
    {
        if (blendBlink < 100f)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(1, blendBlink);
            blendBlink += blendSpeed;
        }
    }

    public void BlendHappy()
    {
        if (blendBlink < 100f)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(2, blendBlink);
            blendBlink += blendSpeed;
        }
    }

    public void ResetBlends()
    {
        skinnedMeshRenderer.SetBlendShapeWeight(1, 0);
        skinnedMeshRenderer.SetBlendShapeWeight(2, 0);
    }
}