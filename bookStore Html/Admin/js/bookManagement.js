$(document).ready(function(){
    getCategories("adminSearchCategory");
    getAuthors("adminSearchAuthor");
    getPublisher("adminSearchPublisher");
    loadBooks();
});

let booksPerPage = 10;
let pageIndex = 0;
let bookName = "";
let categoryId = 0;
let AuthorId = 0;
let publisherId = 0;
let lastPageIndex = 0;

$("#admin-um-form-searchbtn").click(searchBook);

function searchBook(){

    pageIndex = 0;
    $("#nextBtn").prop("disabled", false);
    $("#prevBtn").prop("disabled", true);

    bookName = $("#adminSearchName").val();
    categoryId = $("#adminSearchCategory").val();
    AuthorId = $("#adminSearchAuthor").val();
    publisherId = $("#adminSearchPublisher").val();
    loadBooks();

}

function getCategories(id){
    callAjax("GET", "https://localhost:44325/api/Category/Get?pageSize=1000", "").then(function(result){

        console.log(result);

        let container = $("#"+id);
        let data = result.data.records;
        container.empty();
        container.append(`<option value="0" selected>Select Category</option>`);
        for(let i=0;i<data.length; i++){
            container.append(`<option value="${data[i].categoryId}">${data[i].categoryName}</option>`);
        }

    }).catch(function(error){

        console.log(error);

    });

}

function getAuthors(id){

    callAjax("GET", "https://localhost:44325/api/Author/GetAll?pageIndex=0&pageSize=1000", "").then(function(result){

        console.log(result);

        let container = $("#"+id);
        let data = result.data.records;
        container.empty();
        container.append(`<option value="0" selected>Select Author</option>`);
        for(let i=0;i<data.length; i++){
            container.append(`<option value="${data[i].userId}">${data[i].firstName+" "+data[i].lastName}</option>`);
        }

    }).catch(function(error){
        console.log(error);
    });
}

function getPublisher(id){

    callAjax("GET", "https://localhost:44325/api/Publisher/GetAll?pageIndex=0&pageSize=1000", "").then(function(result){

        console.log(result);
        let container = $("#"+id);
        let data = result.data.records;
        container.empty();
        container.append(`<option value="0" selected>Select Publisher</option>`);
        for(let i=0;i<data.length; i++){
            container.append(`<option value="${data[i].userId}">${data[i].firstName +" "+ data[i].lastName}</option>`);
        }

    }).catch(function(error){

        console.log(error);

    });

}

// pagination
$("#prevBtn").click(function(){
    pageIndex--;
    $("#nextBtn").prop("disabled", false);
    if(pageIndex <= 0){
        pageIndex = 0;
        $("#prevBtn").prop("disabled", true);
    }
    loadBooks();
});

$("#nextBtn").click(function(){
    pageIndex++;
    $("#prevBtn").prop("disabled", false);
    if(pageIndex >= lastPageIndex - 1){
        pageIndex = lastPageIndex - 1;
        $("#nextBtn").prop("disabled", true);
    }

    loadBooks();
});

// books per page dropdown
$("#booksPerPage").change(function(){
    pageIndex = 0;
    $("#nextBtn").prop("disabled", false);
    booksPerPageSelect();
    loadBooks();
});

function booksPerPageSelect(){
    booksPerPage = $("#booksPerPage").val();
}

function loadBooks(){

    $("#currentPage").text(pageIndex+1);
    callAjax("GET", `https://localhost:44325/api/Admin/GetAllBooks?pageIndex=${pageIndex}&pageSize=${booksPerPage}&bookName=${bookName}&categoryId=${categoryId}&authorId=${AuthorId}&publisherId=${publisherId}`).then(function(result){
        console.log(result);
 
        lastPageIndex = Math.ceil(result.data.totalRecords / booksPerPage);
        $("#bookManagementTbody").empty();

        let data = result.data.records;

        $("#totalBooks").text(result.data.totalRecords);

        for(let i=0; i<data.length; i++){
            
            let inventory = 0;
            if(data[i].inventory != null)
                inventory = data[i].inventory

            $("#bookManagementTbody").append(`<tr>
            <td>${data[i].bookId}</td>
            <td>
                ${data[i].name}
            </td>
            <td>
                ${data[i].category}
            </td>
            <td>
                ${data[i].author}
            </td>
            <td>
                ${data[i].publisher}
            </td>
            <td class="inventoryColumn">
                ${inventory}
            </td>
            <td>
                ${data[i].price}
            </td>
            <td class="admin-table-action dropdown"> 
                <button class="dropdown-toggle" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor" height="20px" width="20px">
                        <path d="M10 6a2 2 0 110-4 2 2 0 010 4zM10 12a2 2 0 110-4 2 2 0 010 4zM10 18a2 2 0 110-4 2 2 0 010 4z" />
                      </svg>    
                </button>
               
                    <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
                        <li class=""><a class="dropdown-item editBook" role="button" data-value="${data[i].bookId}">Edit</a></li>
                        <li class=""><a class="dropdown-item addInventory" role="button" data-value="${data[i].bookId}" data-inventory="${data[i].inventory}">Add Inventory</a></li>
                        <li class=""><a class="dropdown-item deleteBook" role="button" data-value="${data[i].bookId}">Delete</a></li>
                    </ul>
              
            </td>
        </tr>`);
        }

    }).catch(function(error){
        console.log(error);
    });
}

$("#addBookBtn").click(function(){
    window.location.href="addBook.html";
});

// Action column
$("#bookManagementTbody").click(function(e){

    console.log(e.target.dataset.value);
    if(e.target.classList.contains("editBook")){
        console.log("edit");
        getCategories("category");
        getAuthors("author");
        getPublisher("publisher");
        getBookById(e.target.dataset.value);
        $("#editBookModal").modal("show");

    }

    if(e.target.classList.contains("addInventory")){
        $("#inventorySize").val(e.target.dataset.inventory);

        $("#bookIdInventory").val(e.target.dataset.value);

        $("#addInventoryModal").modal("show");
    }

    if(e.target.classList.contains("deleteBook")){
        console.log("delete");
        deleteBook(e.target.dataset.value);
    }

});

function putValuesToModal(book){
    $("#bookIdForm").val(book.bookId);
    $("#title").val(book.name);
    $("#description").val(book.description);
    $("#price").val(book.price);
    $("#category").val(book.categoryId);
    $("#author").val(book.authorId);
    $("#publisher").val(book.publisherId);
}

function deleteBook(bookId){
    
    callAjax("DELETE", `https://localhost:44325/api/Book/Delete?id=${bookId}`).then(function(result){
        console.log(result);
        loadBooks();
    }).catch(function(error){
        console.log(error);
    });
}

$("#addInventoryModalBtn").click(function(){
    let bookId = $("#bookIdInventory").val();
    let inventorySize = $("#inventorySize").val();

    addInventory(bookId, inventorySize);

});

function addInventory(bookId, inventorySize){
    let data = {};
    data.bookId = bookId;
    data.inventory = inventorySize;
    callAjax("POST", `https://localhost:44325/api/Book/AddInventory`, JSON.stringify(data)).then(function(result){
        console.log(result);
        $("#addInventoryModal").modal("hide");
        loadBooks();
    }).catch(function(error){
        console.log(error);
    });
}

function getBookById(bookId){
    callAjax("GET", `https://localhost:44325/api/Book/GetById?id=${bookId}`).then(function(result){
        console.log(result);
        putValuesToModal(result.data);     
    }).catch(function(error){
        console.log(error);
    });   
}

// input type number
$("#inventorySize").keydown(function(e){
    checkIfNumber(e);
});

function checkIfNumber(event) {

    /**
     * Allowing: Integers | BackSpace | Tab | Delete | Left & Right arrow keys
     **/
 
    const regex = new RegExp(/(^\d*$)|(Backspace|Tab|Delete|ArrowLeft|ArrowRight)/);
         
    return !event.key.match(regex) && event.preventDefault();
 }
 
 // add inventory modal
$("#plusBtn").click(addOne);
$("#minusBtn").click(minusOne);

 function addOne(){
    let inventorySize = parseInt($("#inventorySize").val());
 
    console.log(typeof(inventorySize));
    if(inventorySize == "" || inventorySize == null || Number.isNaN(inventorySize)){
        inventorySize = 0;
        console.log("inside"+ inventorySize);
    }
    inventorySize++;
    $("#minusBtn").prop("disabled", false);
    $("#inventorySize").val(inventorySize);
 }

 function minusOne(){
    let inventorySize = parseInt($("#inventorySize").val());
 
    if(inventorySize == "" || inventorySize == null || Number.isNaN(inventorySize)){
        inventorySize = 0;
    }  
    inventorySize--;
    if(inventorySize <= 0){
        inventorySize = 0;
        $("#minusBtn").prop("disabled", true);
    }
    $("#inventorySize").val(inventorySize);
 }


//  update Book
$("#updateBookBtn").click(function(e){
    e.preventDefault();
    let updateBookForm = document.querySelector("#updateBookForm");
    console.log(updateBookForm);
    let formData= new FormData(updateBookForm);
    for(const [key, value] of formData){
        console.log(key+" "+ value);
    }
    addBookAjax("PUT", "https://localhost:44325/api/Book/Update", formData).then(function(result){
        console.log(result);
    }).catch(function(error){
        console.log(error);
    });

});


const export_button = document.getElementById('export');

export_button.addEventListener('click', () => {
    html_table_to_excel('xlsx',"admin-um-table");
});
