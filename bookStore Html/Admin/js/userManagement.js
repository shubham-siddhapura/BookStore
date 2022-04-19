$(document).ready(function(){
    loadUsers();
});

let usersPerPage = 10;
let pageIndex = 0;
let userName = "";
let email = "";
let roleId = 0;
let lastPageIndex = 0;

$("#admin-um-form-searchbtn").click(searchUser);

function searchUser(){

    pageIndex = 0;
    $("#nextBtn").prop("disabled", false);

    userName = $("#adminSearchName").val();
    email = $("#adminSearchEmail").val();
    roleId = $("#adminSearchRole").val();

    loadUsers();

}

$("#usersPerPage").change(function(){
    pageIndex = 0;
    $("#nextBtn").prop("disabled", false);
    usersPerPageSelect();
    loadUsers();
});

function usersPerPageSelect(){
    usersPerPage = $("#usersPerPage").val();
}

$("#userManagementTbody").click(function(e){
    console.log(e);

    if(e.target.classList.contains("dropdown-item")){
        console.log(e.target.dataset.value);
        let data = {};
        data.userId = e.target.dataset.value;
        callAjax("PUT", "https://localhost:44325/api/Admin/ToggleActivation", JSON.stringify(data)).then(function(result){
            console.log(result);
            loadUsers();
        }).catch(function(error){
            console.log(error);
        });
    }

});

$("#prevBtn").click(function(){
    pageIndex--;
    $("#nextBtn").prop("disabled", false);
    if(pageIndex <= 0){
        pageIndex = 0;
        $("#prevBtn").prop("disabled", true);
    }
    loadUsers();
});


$("#nextBtn").click(function(){
    pageIndex++;
    $("#prevBtn").prop("disabled", false);
    if(pageIndex >= lastPageIndex - 1){
        pageIndex = lastPageIndex - 1;
        $("#nextBtn").prop("disabled", true);
    }

    loadUsers();
});

function loadUsers(){
    
    callAjax("GET", `https://localhost:44325/api/Admin/GetAllUsers?pageIndex=${pageIndex}&pageSize=${usersPerPage}&userName=${userName}&roleId=${roleId}&email=${email}`).then(function(result){

        console.log(result);

        $("#userManagementTbody").empty();
        
        let data = result.data.records;


        $("#totalUsers").text(result.data.totalRecords);

        lastPageIndex = Math.ceil(result.data.totalRecords / usersPerPage);

        $("#currentPage").text(pageIndex+1);

        for(let i=0; i<data.length; i++){
            let date = "";
            if(data[i].createdOn != null){
                dateArr = data[i].createdOn.split("T")[0].split("-");
                date = dateArr[2]+"/"+dateArr[1]+"/"+dateArr[0];
            }

            let activeStatus = `<span class="admin-table-status" style="background-color:#FF6B6B;">Inactive</span>`;

            let actionLabel = "Activate";

            if(data[i].isActive == true){
                activeStatus = `<span class="admin-table-status" style="background-color:#70e73b;">Active</span>`;

                actionLabel = "Deactivate";

            }

            let userType ="";
            if(data[i].roleId == 1){
                userType = "Admin";
            }
            else if(data[i].roleId == 2){
                userType = "User";
            }
            else if(data[i].roleId == 3){
                userType = "Author";
            }
            else{
                userType = "Publisher";
            }

            $("#userManagementTbody").append(`<tr>
            <td>${data[i].userId}</td>
            <td>
                ${data[i].firstName + " " + data[i].lastName}
            </td>
            <td>
                <span><img src="img/upcomingService/calendar2.png" alt=""></span> 
                <span>${date}</span>
            </td>
            <td>
                ${userType}
            </td>
            <td>
                ${data[i].email}
            </td>
            <td>
                ${activeStatus}
            </td>
            <td class="admin-table-action dropdown"> 
                <button class="dropdown-toggle" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor" height="20px" width="20px">
                        <path d="M10 6a2 2 0 110-4 2 2 0 010 4zM10 12a2 2 0 110-4 2 2 0 010 4zM10 18a2 2 0 110-4 2 2 0 010 4z" />
                      </svg>    
                </button>
               
                    <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
                        <li class=""><a class="dropdown-item" role="button" data-value="${data[i].userId}">${actionLabel}</a></li>
                    </ul>
              
            </td>
        </tr>`);
        }

    }).catch(function(error){
        console.log(error);
    })

}

const export_button = document.getElementById('export');

export_button.addEventListener('click', () => {
    html_table_to_excel('xlsx',"admin-um-table");
});