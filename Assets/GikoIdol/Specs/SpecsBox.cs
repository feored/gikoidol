using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecsBox : MonoBehaviour
{
    [SerializeField]
    private IdolSpecs specs;

    [SerializeField]
    private TextMeshProUGUI name;

    [SerializeField]
    private TextMeshProUGUI trait;

    [SerializeField]
    private TextMeshProUGUI hobby;

    [SerializeField]
    private TextMeshProUGUI age;

    

    public void Init(IdolSpecs specs){
        this.specs = specs;
    }

    public void DrawSpecs(){
        this.name.text = this.specs.Name;
        this.trait.text = this.specs.Trait;
        this.hobby.text = this.specs.Hobby;
        this.age.text = this.specs.Age.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
