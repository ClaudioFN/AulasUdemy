var parent = {
    value: "parentValue",
    obj: {
        objValue: "parentObjValue"
    },
    walk: function (){
        console.log("Walking");
    }
};

var child = Object.create(parent);
console.log("CHILD - child.value: ", child.value);
console.log("CHILD - child.obj.objValue: ", child.obj.objValue);
console.log("PARENT - parent.value: ", parent.value);
console.log("PARENT - parent.obj.objValue: ", parent.obj.objValue);
console.log("parent: ", parent);
console.log("child: ", child);

var student1 = {
    message: "I LOVE this course!"
  };
  
  var student2 = Object.create(student1);
  console.log(student2.message);