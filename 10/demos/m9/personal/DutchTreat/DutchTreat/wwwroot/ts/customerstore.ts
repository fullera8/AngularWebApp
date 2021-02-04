// The export keyword stops this class from being global and requires importing where needed
// export class StoreCustomer {
class StoreCustomer {

    // property types are not required, you can let the interpreter infer it or explicitly pass it with ":"
    public visits: number = 0;
    private ourName: string;

    // Constructors require the "constructor" keyword but otherwise follow the function format
    // You can declare private properties in the params of the constructor
    // that will automatically make them available to the class
    constructor(private firstName: string, private lastName: string) {
    }

    // properties need to be declared then explicitly referred to using "this."
    set name(value) {
        this.ourName = value;
    }
    get name() {
        return this.ourName;
    }

    // Function does not explicitly need to be called a function, interpreted by the signature
    // public showName(name: string): boolean {
    //    alert(name);
    //    return true;
    // }
    public showName() {
        alert(this.firstName + " " + this.lastName); // ctor properties are wired
    }
}

// let cust = new StoreCustomer();
// cust.visits = "not a number"; //Example of type safety
