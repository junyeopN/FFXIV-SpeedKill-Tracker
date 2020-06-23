var submitButton = document.getElementById("todosubmitbutton");
var inputTodo = document.getElementById("todo");

var newTodo = "";
var todoItems = [];

submitButton.onclick = function () {
    newTodo = inputTodo.value;
    todoItems.push(newTodo);
    addNewItemToTodoList(todoItems);
}

function addNewItemToTodoList(itemList) {
    alert(itemList.length);
}