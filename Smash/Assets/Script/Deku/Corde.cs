using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corde : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private GameObject _bout;
    public bool isActivate;

    [SerializeField]
    private GameObject skillObject;

    [SerializeField]
    private float skillForce;

    [SerializeField]
    private Charac_Deku deku;

    private GameObject currentSkill;

    
    public float speed = 10f;


    void Update()
    {

        if (currentSkill == null && isActivate)
        {
            resetComp();
        }
        if (currentSkill != null)
        {
            if (currentSkill.GetComponent<Debut_fouet>().iscollid && !isActivate)
            {
                Grab();
            }
        }

        if (isActivate)
        {
            moveTo();
        }

        
    }

    public void Setbout(GameObject bout)
    {
        _bout = bout;
    }

    public void StartFouet()
    {
        currentSkill = Instantiate(skillObject, deku.selectedHitbox.transform.position, deku.selectedHitbox.transform.rotation);



        if (deku.selectedHitbox == deku.hitboxRight)
        {
            currentSkill.GetComponent<Rigidbody>().AddForce(Vector3.forward * skillForce, ForceMode.Impulse);
        }
        else if (deku.selectedHitbox == deku.hitboxLeft)
        {
            currentSkill.GetComponent<Rigidbody>().AddForce(-Vector3.forward * skillForce, ForceMode.Impulse);
        }
        else
        {
            currentSkill.GetComponent<Rigidbody>().AddForce(Vector3.up * skillForce, ForceMode.Impulse);
        }



        Destroy(currentSkill, 3);
    }

    void Grab()
    {
        isActivate = true;
        GetComponent<PlayerController>().isMove = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<PlayerController>().gravityMulti = 1f;
        GetComponent<LineRenderer>().enabled = true;
        GetComponent<LineRenderer>().SetPosition(1, currentSkill.transform.position);

    }
    void moveTo()
    {
        transform.position = Vector3.Lerp(transform.position, currentSkill.transform.position, speed * Time.deltaTime / Vector3.Distance(transform.position, currentSkill.transform.position));

        GetComponent<LineRenderer>().SetPosition(0, transform.position);

        if(Vector3.Distance(transform.position, currentSkill.transform.position) < 1f)
        {
            resetComp();
        }

    }

    void resetComp()
    {
        isActivate = false;
        GetComponent<PlayerController>().isMove = true;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<PlayerController>().gravityMulti = 3f;
        GetComponent<LineRenderer>().enabled = false;
        if(currentSkill != null)
        {
            Destroy(currentSkill);
        }
    }

}
