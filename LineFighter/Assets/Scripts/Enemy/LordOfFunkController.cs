using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfFunkController : EnemyAI
{
    #region Member Variables..
    private float _staffBurstCd = 0f;
    #endregion Member Variables..

    #region Properties..
    #endregion Properties..

    #region Events..
    #region MonoBehaviour..
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        // Staff burst
        if (_staffBurstCd > 5)
        {
            StartCoroutine("StaffSpiralQuad");
            _staffBurstCd = 0;
        }
        else
        {
            _staffBurstCd += Time.deltaTime;
        }
    }
    #endregion MonoBehaviour.. 
    #endregion Events..

    #region Public Methods..
    public override void InitializeProperties()
    {
        base.InitializeProperties();

        this.Hp = 100;
    }
    #endregion Public Methods..

    #region Private Methods..
    private void CharacterBurst()
    {
        // Ring of projectiles
        for (int i = 1; i < 13; i++)
        {
            float point = (i * 1.0f) / 15;
            float angle = (point * Mathf.PI * 2f) + 0.4f;

            Vector3 position = new Vector3();
            position.x = (Mathf.Sin(angle) + 5f * this.gameObject.transform.position.x) - 2f;
            position.y = Mathf.Cos(angle) * this.gameObject.transform.position.y;

            //Quaternion rot = Quaternion.FromToRotation(Vector3.forward, this.transform.position - pos);

            position = position + this.gameObject.transform.position;

            Vector3 direction = Vector3.zero;

            FireProjectile(position, direction);
        }
    }

    private IEnumerator StaffBeam()
    {
        Vector3 lookAtPos = new Vector3();
        Vector3 position = new Vector3();

        for (int i = 0; i < 50; i++)
        {
            float point = (i * 1.0f) / 50;
            float angle = point * Mathf.PI * 2f;

            lookAtPos.x = (Mathf.Sin(angle) + 2f * this.gameObject.transform.position.x);
            lookAtPos.y = (Mathf.Cos(angle) + 2f * this.gameObject.transform.position.y);

            position.x = lookAtPos.x / 2f;
            position.y = lookAtPos.y / 2f;

            position = position + this.gameObject.transform.position;

            FireProjectile(position, lookAtPos);

            yield return null;
        }
    }

    private IEnumerator StaffSpiral(int startingIndex, float timeBetween)
    {
        Vector3 lookAtPos = new Vector3();
        Vector3 position = new Vector3();

        for (int i = startingIndex; i < startingIndex + 150; i++)
        {
            float point = (i * 1.0f) / 50;
            float angle = point * Mathf.PI * 2f;

            lookAtPos.x = (Mathf.Sin(angle) + 2f * this.gameObject.transform.position.x);
            lookAtPos.y = (Mathf.Cos(angle) + 2f * this.gameObject.transform.position.y);

            position.x = lookAtPos.x / 2f;
            position.y = lookAtPos.y / 2f;
            
            position = position + this.gameObject.transform.position;

            FireProjectile(position, lookAtPos);

            yield return new WaitForSeconds(timeBetween);
        }
    }

    private void StaffSpiralQuad()
    {
        IEnumerator staffSpiralCoroutineOne = StaffSpiral(0, 0.1f);
        IEnumerator staffSpiralCoroutineTwo = StaffSpiral(37, 0.1f);
        IEnumerator staffSpiralCoroutineThree = StaffSpiral(74, 0.1f);
        IEnumerator staffSpiralCoroutineFour = StaffSpiral(111, 0.1f);

        StartCoroutine(staffSpiralCoroutineOne);
        StartCoroutine(staffSpiralCoroutineOne);
        StartCoroutine(staffSpiralCoroutineOne);
        StartCoroutine(staffSpiralCoroutineOne);
    }

    private void StaffBurst()
    {
        Vector3 lookAtPos = new Vector3();
        Vector3 position = new Vector3();

        // Ring of projectiles
        for (int i = 0; i < 50; i++)
        {
            float point = (i * 1.0f) / 50;
            float angle = point * Mathf.PI * 2f;

            lookAtPos.x = (Mathf.Sin(angle) + 2f * this.gameObject.transform.position.x);
            lookAtPos.y = (Mathf.Cos(angle) + 2f * this.gameObject.transform.position.y);

            position.x = lookAtPos.x / 2f;
            position.y = lookAtPos.y / 2f;

            position = position + this.gameObject.transform.position;

            FireProjectile(position, lookAtPos);
        }
    }
    #endregion Private Methods..
}
