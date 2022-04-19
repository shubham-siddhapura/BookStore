let booksPerPage = 6;
let totalPage = 0;
let currentPage = 0;
let category = "";
let keyword = "";
let url = new URLSearchParams(window.location.search);
    
$(document).ready(function(){
    
    getCategory();
    //getBooks();
    
});

function getCategory(){
    callAjax("GET", "https://localhost:44325/api/Category/Get?pageSize=1000", "").then(function(result){
        console.log(result);
        $("#categoryDropdown").empty();

        let data = result.data.records;

        $("#categoryDropdown").append('<option value="">All</option>');

        for(let i = 0 ; i< data.length; i++){
            $("#categoryDropdown").append(`
            <option value="${data[i].categoryId}">${data[i].categoryName}</option>
            `);
        }

        
   
    if(url != null){
        console.log(url.toString());
        if(url.toString().split("=")[0] == "category"){
            let urlCategoryId = url.toString().split("=")[1];
            console.log("cat id: "+urlCategoryId);
            
            $("#categoryDropdown").val(urlCategoryId);
            category= urlCategoryId;
        }
        else if(url.toString().split("=")[0] == "search"){
            let urlSearch = url.toString().split("=")[1];
            $("#searchBooks").val(urlSearch);
            keyword = urlSearch;
        }
    }

    // get books as per selected category
    getBooks();

    }).catch(function(error){
        console.log(error);
    });
}

$("#booksPerPage").change(function(){

    console.log("changed");
    booksPerPage = $("#booksPerPage").val();
    getBooks();
    console.log(booksPerPage);
});

$("#categoryDropdown").change(function(){
    console.log($("#categoryDropdown").val());
    category = $("#categoryDropdown").val();
    getBooks();
})

$("#prevBtn").click(function(){
    if(currentPage > 0){
        currentPage--;
        $("#nextBtn").prop("disabled", false);
        getBooks();
        if(currentPage <= 0){
            $("#prevBtn").prop("disabled", true);
        }
    }
    else{
        $("#prevBtn").prop("disabled", true);
    }
});

$("#nextBtn").click(function(){

    if(currentPage < totalPage - 1){
        currentPage++;
        $("#prevBtn").prop("disabled", false);
        getBooks();
        if(currentPage >= totalPage -1){
            $("#nextBtn").prop("disabled", true);
        }
    }
    else{
        $("#nextBtn").prop("disabled", true);
    }
});

$("#searchBooks").keyup(function(){
    keyword = $("#searchBooks").val();
    console.log(keyword);
});

$("#searchBtn").click(getBooks);

function getBooks(){
    
    let data = {};
    data.pageIndex = currentPage;
    data.pageSize = booksPerPage;
    data.keyword = keyword;  
    data.category = category;
    
    callAjax("GET", "https://localhost:44325/api/Book/GetAll", data).then(function (result){
        console.log(result);
        $("#booksDiv").empty(); 
        let data = result.data.records;

        totalPage = Math.ceil(result.data.totalRecords / booksPerPage);

        if(data.length == 0){
            $("#noBookAvailable").removeClass("d-none");
            if(category != "")
                $("#noBookAvailable h2").text("There is no book available for ' "+$("#categoryDropdown option:selected").text()+ "' category.");
        }
        else{
            $("#noBookAvailable").addClass("d-none");
        }

        for(let i=0; i<data.length; i++){
            $("#booksDiv").append(`
            <div class="book">
                <div class="bookImage">
                    <img src="data: image/jpg;base64, ${data[i].getImage.fileContents}" alt="">
                </div>
                <div class="bookTitle">
                    <span>`+data[i].name+`</span>
                </div>
                <div class="price">&#8377; <span>`+data[i].price+`</span></div>
                <div class="addToCart">
                    <button class="addToCartBtn" data-value="`+data[i].bookId+`">Add To Cart</button>
                </div>

                <div class="favourite">
                    <input type="checkbox" id="favourite`+data[i].bookId+`" class="favouriteCheckBox" value="`+data[i].bookId+`"/>
                    <label for="favourite`+data[i].bookId+`">★</label>
                </div>
            </div>
            `);
        }
        $("#totalRecords").text(result.data.totalRecords);

        if(totalPage > 1){
            $("#nextBtn").prop("disabled", false);
        }

        if(currentPage >= totalPage-1){
            $("#nextBtn").prop("disabled", true);
        }

        if(currentPage > 0){
            $("#prevBtn").prop("disabled", false);
        }
        else{
            $("#prevBtn").prop("disabled", true);
        }

    }).catch(function(error){
        console.log(error);
    });

}


// on click add to cart

$("#booksDiv").click(function(e){

    console.log(e);
    if(e.target.classList.contains("addToCartBtn")){

        // login or not
        if(isLoggedin()){

            let cookie = document.cookie.split("=")[1];
            let cookieObj = JSON.parse(cookie);
            

            let data = {};
            data.bookId = e.target.dataset.value;
            data.orderBy = cookieObj.userId;
            
            callAjax("POST", "https://localhost:44325/api/Order/Add", JSON.stringify(data)).then(function(result){
                console.log(result);
                $("#bookOrderAlert").removeClass("d-none").fadeIn().fadeOut(7000);
            }).catch(function(error){
                console.log(error);
            });

        }
        else{
            window.location.href = "index.html?loginModal=true";
        }     

        console.log(e.target.dataset.value);
    }

});

// query string execution



 // fetch('https://localhost:44325/api/Book/GetAll?pageIndex=0&pageSize=6&keyword', {
    // method: 'GET', 
    // mode: 'no-cors', 
    // credentials: 'include', 
    // headers: {
    //   'Content-Type': 'application/json',
    //   // 'Content-Type': 'application/x-www-form-urlencoded',
    //   'Access-Control-Allow-Origin': 'https://javascript.info' 
    // },
    // Vary : 'Origin'
    // })
    // .then(function(response){
    //     console.log(response);
    // })
    // .catch(function(error){
    //     console.log('Looks like there was a problem: ', error);

    // });
  