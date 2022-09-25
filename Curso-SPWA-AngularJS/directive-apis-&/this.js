function Person() {
    this.fullName = "Claudio";
    this.fav = "Cookies";
  
    this.describe = function () {
      console.log('this is: ', this);
      console.log(this.fullName + " likes " + this.fav);
    };
  }
  
  var claudio = new Person();
  claudio.describe();
  
  var describe = claudio.describe;
  describe();
  describe.call(claudio);