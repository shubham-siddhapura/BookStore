$(document).ready(function(){
    getOrders();
});

let ordersPerPage = 10;
let pageIndex = 0;
let bookName = "";
let userName = "";
let orderStatus = 0;
let lastPageIndex = 0;

$("#adminSearchBook").change(function(){
    bookName = $("#adminSearchBook").val();
});

$("#adminSearchName").change(function(){
    userName = $("#adminSearchName").val();
});

$("#adminSearchStatus").change(function(){
    orderStatus = $("#adminSearchStatus").val();
});

$("#admin-um-form-searchbtn").click(function(){
    pageIndex = 0;
    getOrders();
});

$("#ordersPerPage").change(function(){
    ordersPerPage = $("#ordersPerPage").val();
    getOrders();
});

function getOrders(){
    callAjax("GET", `https://localhost:44325/api/Order/GetAll?pageIndex=${pageIndex}&pageSize=${ordersPerPage}&bookName=${bookName}&userName=${userName}&status=${orderStatus}`).then(function(result){

        console.log(result);

        $("#currentPage").text(pageIndex + 1);

        $("#totalOrders").text(result.data.totalRecords);

        lastPageIndex = Math.ceil(result.data.totalRecords / ordersPerPage);

        $("#orderManagementTbody").empty();
        let data = result.data.records;
        for(let i=0; i<data.length; i++){
            let bgColor = "";
            let statusLabel = "";
            let dropdwonItems = "";
            let dNone = "";
            if(data[i].status == 1){
                statusLabel = "Ordered";
                bgColor = "#745485";
                dropdwonItems = `<li><a class="dropdown-item" data-id="${data[i].orderId}" data-value="2">Dispatch</a></li>
                <li><a class="dropdown-item" data-id="${data[i].orderId}" data-value="3">Deliver</a></li>
                <li><a class="dropdown-item" data-id="${data[i].orderId}" data-value="4">Cancel</a></li>`;
            }
            else if(data[i].status== 2){
                statusLabel = "Dispatched";
                bgColor = "#1877e5";
                dropdwonItems = `
                <li><a class="dropdown-item" data-id="${data[i].orderId}" data-value="3">Deliver</a></li>
                <li><a class="dropdown-item" data-id="${data[i].orderId}" data-value="4">Cancel</a></li>`;
            }
            else if(data[i].status == 3){
                statusLabel = "Delivered";
                bgColor = "#1bab6e";
                dNone = "d-none";
            }
            else{
                statusLabel = "Cancelled";
                bgColor = "#FF6B6B";
                dNone = "d-none";
            }
            $("#orderManagementTbody").append(`
            <tr>
                    <td>${data[i].orderId}</td>
                    <td>
                        ${data[i].orderByNavigation.firstName + " " + data[i].orderByNavigation.lastName}
                    </td>
                    <td>
                        ${data[i].book.name}
                    </td>
                    <td>
                        <span class="admin-table-status" style="background-color:${bgColor};">${statusLabel}</span> 
                    </td>
                    <td class="admin-table-action dropdown"> 
                        <button type="button" class="admin-table-actionbtn" data-bs-toggle="dropdown" >
                            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor" height="20px" width="20px">
                                <path d="M10 6a2 2 0 110-4 2 2 0 010 4zM10 12a2 2 0 110-4 2 2 0 010 4zM10 18a2 2 0 110-4 2 2 0 010 4z" />
                                </svg>    
                        </button>
                        
                            <ul class="dropdown-menu ${dNone}">
                               ${dropdwonItems}
                            </ul>
                    
                    </td>
                </tr>
            `);
        }

    }).catch(function(error){
        console.log(error);
    });
}

$("#orderManagementTbody").click(function(e){
    if(e.target.classList.contains("dropdown-item")){
        console.log(e.target.dataset.value);
        console.log(e.target.dataset.id);  
        changeStatus(e.target.dataset.value, e.target.dataset.id);     
    }
});

function changeStatus(status, id){
    let data={};
    data.orderId = id;
    data.status = status;
    callAjax("POST", "https://localhost:44325/api/Order/ChangeStatus", JSON.stringify(data)).then(function(result){
        console.log(result);
        getOrders();       
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
    getOrders();
});

$("#nextBtn").click(function(){
    pageIndex++;
    $("#prevBtn").prop("disabled", false);
    if(pageIndex >= lastPageIndex - 1){
        pageIndex = lastPageIndex - 1;
        $("#nextBtn").prop("disabled", true);
    }

    getOrders();
});
