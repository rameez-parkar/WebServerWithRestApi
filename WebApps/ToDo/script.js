var count=0;

function InitialTodoDefault(){
  const FETCH_COUNT = 10;
  fetch('https://jsonplaceholder.typicode.com/todos/')
    .then(response => response.json())
    .then(function(data){
      for(let i=1; i<FETCH_COUNT; i++)
      {
        Add(data[i].title);
      }
    })
}

function Add(data){
  if(data != "")
  {
    var table = document.getElementById('todotable');
    var row = table.insertRow();
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var label = document.createElement("label");
    label.innerHTML = data;
    label.setAttribute('id',count);
    cell1.append(label);

    var btn1 =document.createElement('button');
    btn1.setAttribute('class','delete');
    btn1.setAttribute('onclick','Delete('+count+')');
    btn1.innerHTML="Delete";

    var btn2 =document.createElement('button');
    btn2.setAttribute('class','update');
    btn2.setAttribute('onclick','Update('+count+')');
    btn2.innerHTML="Update";
    cell2.append(btn1);
    cell2.append(btn2);
    count++;

    document.getElementById('todoitem').value = "";
  }
  else {
    window.alert("Please enter an activity to add to To Do List...");
  }
  DisplayAll();
}

function DisplayAll(){
  var table = document.getElementById('todotable');
  var tr = table.getElementsByTagName('tr');
  for(var i=0; i<tr.length; i++)
  {
    tr[i].style.backgroundColor = "white";
  }
}

function Search(){
  var table = document.getElementById('todotable');
  var tr = table.getElementsByTagName('tr');
  var str = document.getElementById('todoitem').value.toUpperCase();

  for(var i=0; i<tr.length; i++)
  {
    var td = tr[i].getElementsByTagName("td")[0];
    if (td) {
      var txtValue = td.textContent || td.innerText;
      if (txtValue.toUpperCase().indexOf(str) > -1) {
        tr[i].style.backgroundColor = "powderblue";
      } else {
        tr[i].style.backgroundColor = "white";
      }

      if(str.length < 1)
      {
        tr[i].style.backgroundColor = "white";
      }
    }
  }
}

function Delete(id){
  var table = document.getElementById(id).parentElement.parentElement.parentElement;
  table.removeChild(document.getElementById(id).parentElement.parentElement);
}

function Update(id){
  var label = document.getElementById(id);
  var item = window.prompt("Update the activity...", label.innerHTML);
  if(item!=""){
    label.innerHTML = item;
  }
  else{
    window.alert("Please enter an activity to add to To Do List...");
  }
}

function Reset(getid){
  getid.style.backgroundColor="white";
}

function Validation(getid, getevent){
  var isNameIncorrect=false, isContactIncorrect=false, isDOBIncorrect=false, isEmailIncorrect=false;
  getevent.preventDefault();
  var id = getid.getAttribute('id');
  var elements = document.querySelectorAll("#"+id+" input");
  for(var i=0,element; element=elements[i++];)
  {
    if(element.id === "name")
    {
      var checkNumber = /[0-9]/g;
      if(element.value.length==0 || element.value.match(checkNumber))
      {
        isNameIncorrect = true;
        element.style.backgroundColor="#CD5C5C";
      }
    }
    else if(element.id === "contact")
    {
      var checkChar = /[a-zA-Z]/g;
      if(element.value.length!=10 || element.value.match(checkChar))
      {
        isContactIncorrect = true;
        element.style.backgroundColor="#CD5C5C";
      }
    }
    else if(element.id === "email")
    {
      if(element.value.length==0)
      {
        isEmailIncorrect = true;
        element.style.backgroundColor="#CD5C5C";
      }
    }
    else if(element.id === "dob")
    {
      if(element.value=="mm/dd/yyyy")
      {
        isDOBIncorrect = true;
        element.style.backgroundColor="#CD5C5C";
      }
    }
  }
  if(isNameIncorrect==false && isContactIncorrect==false && isDOBIncorrect==false && isEmailIncorrect==false)
  {
    window.alert("Your Data has been submitted!");
  }
  else
  {
    console.log("dsdsd");
  }
}
