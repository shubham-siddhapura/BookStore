$(document).ready(function(){
    getCategory();
    getAuthor();
    getPublisher();
});



function getCategory(){
    callAjax("GET", "https://localhost:44325/api/Category/Get?pageSize=1000", "").then(function(result){

        console.log(result);
        let container = $("#category");
        let data = result.data.records;
        container.empty();
        container.append(`<option selected>Open this select menu</option>`);
        for(let i=0;i<data.length; i++){
            container.append(`<option value="${data[i].categoryId}">${data[i].categoryName}</option>`);
        }

    }).catch(function(error){

        console.log(error);

    });
}

function getAuthor(){
    callAjax("GET", "https://localhost:44325/api/Author/GetAll?pageIndex=0&pageSize=1000", "").then(function(result){

        console.log(result);

        let container = $("#author");
        let data = result.data.records;
        container.empty();
        container.append(`<option selected>Open this select menu</option>`);
        
        for(let i=0;i<data.length; i++){
            container.append(`<option value="${data[i].userId}">${data[i].firstName+" "+data[i].lastName}</option>`);
        }

    }).catch(function(error){

        console.log(error);

    });

}

function getPublisher(){
    callAjax("GET", "https://localhost:44325/api/Publisher/GetAll?pageIndex=0&pageSize=1000", "").then(function(result){

        console.log(result);
        let container = $("#publisher");
        let data = result.data.records;
        container.empty();
        container.append(`<option selected>Open this select menu</option>`);
        for(let i=0;i<data.length; i++){
            container.append(`<option value="${data[i].userId}">${data[i].firstName +" "+ data[i].lastName}</option>`);
        }

    }).catch(function(error){
        console.log(error);
    });
}

$("#addNewBookBtn").click(function(e){
    e.preventDefault();
    let addNewBookForm = document.querySelector("#addNewBookForm");
    console.log(addNewBookForm);
    let formData= new FormData(addNewBookForm);
    console.log(formData);
    console.log(formData.keys());
    for(const [key, value] of formData){
        console.log(key+" "+ value);
    }
    addBookAjax("POST", "https://localhost:44325/api/Book/Add", formData).then(function(result){
        console.log(result);
    }).catch(function(error){
        console.log(error);
    });

});

// let base64String = "";

// $("#formFile").change(imageUploaded);

// function imageUploaded() {
//     var file = document.querySelector(
//         'input[type=file]')['files'][0];
  
//     var reader = new FileReader();
//     console.log("next");
      
//     reader.onload = function () {
//         base64String = reader.result.replace("data:", "")
//             .replace(/^.+,/, "");
  
//         imageBase64Stringsep = base64String;
  
//         // alert(imageBase64Stringsep);
//         console.log(base64String);
//     }
//     reader.readAsDataURL(file);
// }
