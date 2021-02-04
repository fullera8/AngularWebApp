// The export keyword stops this class from being global and requires importing where needed
// export class StoreCustomer {
var StoreCustomer = /** @class */ (function () {
    // Constructors require the "constructor" keyword but otherwise follow the function format
    // You can declare private properties in the params of the constructor
    // that will automatically make them available to the class
    function StoreCustomer(firstName, lastName) {
        this.firstName = firstName;
        this.lastName = lastName;
        // property types are not required, you can let the interpreter infer it or explicitly pass it with ":"
        this.visits = 0;
    }
    Object.defineProperty(StoreCustomer.prototype, "name", {
        get: function () {
            return this.ourName;
        },
        // properties need to be declared then explicitly referred to using "this."
        set: function (value) {
            this.ourName = value;
        },
        enumerable: false,
        configurable: true
    });
    // Function does not explicitly need to be called a function, interpreted by the signature
    // public showName(name: string): boolean {
    //    alert(name);
    //    return true;
    // }
    StoreCustomer.prototype.showName = function () {
        alert(this.firstName + " " + this.lastName); // ctor properties are wired
    };
    return StoreCustomer;
}());
// let cust = new StoreCustomer();
// cust.visits = "not a number"; //Example of type safety
//# sourceMappingURL=customerstore.js.map