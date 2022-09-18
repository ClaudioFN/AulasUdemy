var numberArray = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
console.log("Numer Array: ", numberArray);

function above5Filter(value){
    return value > 5;
}

var filteredNumberArray = numberArray.filter(above5Filter);
console.log("Filtered Number Array: ", filteredNumberArray);

var shoppingList = [
    "Milk", "Chocolate", "Sugar", "Rice", "Beans", "Apple", "Banana", "Cheese", "Bread"
];
var searchValue = "ea";
function containsFilter (value){
    return value.indexOf(searchValue) !== -1;
}

var searchedShoppingList = shoppingList.filter(containsFilter);
console.log("Searched Shopping List: ", searchedShoppingList);