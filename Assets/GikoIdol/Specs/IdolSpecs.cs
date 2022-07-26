using System.Collections;
using System.Collections.Generic;

public class IdolSpecs
{
    public int Age;
    public string Trait;
    public string Name;
    public string Hobby;
    
    
    public IdolSpecs(){
        Age = 0;
        Trait = "";
        Name = "";
        Hobby = "";
    }

    public IdolSpecs(int age, string trait, string name, string hobby)
    {
        this.Age = age;
        this.Trait = trait;
        this.Name = name;
        this.Hobby = hobby;
    }
}
