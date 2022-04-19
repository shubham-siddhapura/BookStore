let userId = 0;
let role = ""
$(document).ready(function(){ 
    
    let cookie = document.cookie.split("=")[1];
    let cookieObj = "";
    if(isLoggedin()){
        cookieObj = JSON.parse(cookie);
        userId = cookieObj.userId;
        role = cookieObj.role;
    }
    
    getUserDetails();
    getMyOrders();
    
});


function getUserDetails(){
    
    callAjax("GET", `https://localhost:44325/api/BookStore/GetById?id=${userId}`).then(function(result){
        console.log(result);

        $("#userName").text(result.data.firstName+" "+ result.data.lastName);
        $("#userEmail").text(result.data.email);
        $("#userRole").text(role);
        $("#userRole").addClass(role+"Tag");

        $("#editFirstName").val(result.data.firstName);
        $("#editLastName").val(result.data.lastName);

    }).catch(function(error){
        console.log(error);
    });
}

function getMyOrders(){
    callAjax("GET", `https://localhost:44325/api/Order/OrderedBook?userId=${userId}`).then(function(result){
        console.log(result);
        
        $("#bookList").empty();

        if(result.data.totalRecords==0){

        }
        else{
            let data = result.data.records;
            for(let i=0; i<data.length; i++){

                let statusClass ="";
                let statusTab = "";
                if(data[i].orderStatus == 1){
                    statusClass = "ordered";
                    statusTab = "Ordered...";
                }
                else if(data[i].orderStatus == 2){
                    statusClass = "dispatched";
                    statusTab = "Dispatched...";
                }
                else if(data[i].orderStatus == 3){
                    statusClass = "delivered";
                    statusTab = "Delivered";
                }
                else{
                    statusClass = "text-danger";
                    statusTab = "Cancelled";
                }

                $("#bookList").append(`
                <!-- book start -->
                <div class="book">
                  <div class="bookImage">
                      <img src="data:image/jpg;base64, ${data[i].getImage.fileContents}" alt="">
                  </div>
                  <div class="bookTitle">
                      <span>${data[i].name}</span>
                  </div>
                  <div class="price">&#8377; <span>${data[i].price}</span></div>
                  
                  <div class="orderStatus">
                      <p class="${statusClass}">${statusTab}</p>
                  </div>
  
                  <div class="favourite">
                      <input type="checkbox" id="favourite1" class="favouriteCheckBox"/>
                      <label for="favourite1">★</label>
                  </div>
                </div>
                <!-- book ends -->
                `);
            }
        }

    }).catch(function(error){
        console.log(error)
    });  
}

$("#editNameBtn").click(function(){
    let data ={};
    data.firstName = $("#editFirstName").val();
    data.lastName = $("#editLastName").val();
    data.id = userId;
    callAjax("PUT", "https://localhost:44325/api/BookStore/UpdateUser", JSON.stringify(data)).then(function(result){
        console.log(result);
        $("#editModal").modal("hide");
        window.location.reload();
    }).catch(function(error){
        console.log(error);
    });
});

$("#changePwdBtn").click(function(){
    let data = {};
    data.id = userId;
    data.oldPwd = $("#oldPwd").val();
    data.newPwd = $("#newPwd").val();
    let confirmPwd = $("#confirmPwd").val();

    if(confirmPwd != data.newPwd){
       $("#changePwdAlert").addClass("alert-danger").removeClass("d-none alert-success").text("New Password and confirm password must be same!!").fadeIn().fadeOut(7000);
    }
    else{
        callAjax("POST", "https://localhost:44325/api/BookStore/ChangePassword", JSON.stringify(data)).then(function(result){
            console.log(result);
            $("#changePwdAlert").addClass("alert-success").removeClass("d-none alert-danger").text("Password has been changed successfully!!").fadeIn().fadeOut(7000);
        }).catch(function(error){

        });
    }
});