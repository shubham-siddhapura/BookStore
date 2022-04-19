$(document).ready(function(){
    //handle Query string
    const url = new URLSearchParams(window.location.search);

    if(url == "loginModal=true"){
        
        $("#loginModal").modal('show');
        console.log("clicked")
    }

    if(url == "logoutModal=true"){
        $("#logoutModal").modal("show");
    }
});

$("#loginBtn").click(login);

function login(){

    let data={};
    data.email = $("#loginEmail").val();
    data.password = $("#loginPassword").val();

    callAjax("POST", "https://localhost:44325/api/BookStore/Login", JSON.stringify(data)).then(function(result){
        console.log(result);

        //set token cookie
        // document.cookie = "token = "+result.data.token;
        // document.cookie = "userId="+result.data.id;
        // document.cookie = "role="+result.data.roleName;

        let myCookie = {};
        myCookie.token = result.data.token;
        myCookie.userId = result.data.id;
        myCookie.role = result.data.roleName;

        console.log(myCookie);

        document.cookie = "myCookie="+JSON.stringify(myCookie);

        console.log(document.cookie);
        console.log(jwtToken);

        $("#loginLi").addClass("d-none");
        $("#logoutLi").removeClass("d-none");
        $("#registerLi").addClass("d-none");
        $("#myProfileBtn").removeClass("d-none");
        $("#loginModal").modal("hide");

        if(result.data.roleId == 1){
            window.location.href = "Admin/userManagement.html";
        }

    }).catch(function(error){
        console.log(error);
    });

}

$("#logoutBtn").click(logout);

function logout(){
    
    window.location.href = "index.html";

    var date = new Date();
    date.setTime(date.getTime()+((-1)*24*60*60*1000));
    document.cookie = "myCookie=''; expires="+date.toGMTString();
    // document.cookie = "token" + "=" + "" + "; expires=" + date.toGMTString();
    // document.cookie = "userId" + "=" + "" + "; expires=" + date.toGMTString();
    // document.cookie = "role" + "=" + "" + "; expires=" + date.toGMTString();

    // jwtToken = document.cookie.split("=")[0];
    // console.log(jwtToken);

    console.log(document.cookie);

    $("#myProfileBtn").addClass("d-none");
    $("#loginLi").removeClass("d-none");
    $("#logoutLi").addClass("d-none");
    $("#registerLi").removeClass("d-none");   
    $("#logoutModal").modal("hide");

}


function slideMove(direction){

    let numberOfSlide = $(".slide").length;

    let slideWidth = $(document).width();

    let appliedMargin = Math.abs(parseInt($("#slides").css("margin-left")));

    let calculateRight = appliedMargin + slideWidth;
    let calculateLeft = appliedMargin - slideWidth
    
    let lastSlide = slideWidth * numberOfSlide - slideWidth;

    let goRight = "-"+calculateRight+"px";
    
    let goLeft = "-"+calculateLeft+"px";

    let goLast = "-"+lastSlide+"px";

    console.log("num "+numberOfSlide);

    if(direction == "right"){

        if(calculateRight >= slideWidth * numberOfSlide){
        
            $("#slides").animate({marginLeft: 0}, 500);
        }
        else{
            $("#slides").animate({marginLeft: goRight}, 500);  
        }
    }
    else{

        if(calculateLeft<0){
            $("#slides").animate({marginLeft: goLast}, 500);
        }
        else{
            $("#slides").animate({marginLeft: goLeft}, 500);
        }

    }
}

$("#leftMove").click(function(){
    slideMove("left");
});


$("#rightMove").click(function(){
    slideMove("right");
});


// auto slider 
setInterval(function(){
    slideMove("right");
}, 7000);

// search bar on slider 
$("#searchBtn").click(function(){
    let search = $.trim($("#searchBooks").val());
    if(search != ""){
        window.location.href = "books.html?search="+search;
    }
});