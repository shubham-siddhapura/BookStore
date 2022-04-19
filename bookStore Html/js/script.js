let jwtToken ="";

// Ajax call 
function callAjax(type, url, data){
    //get token from cookie    
    let cookie = document.cookie.split("=")[1];
    let cookieObj = "";
    if(isLoggedin()){
        let cookieObj = JSON.parse(cookie);
        console.log(cookieObj);
        jwtToken ="Bearer " + cookieObj.token;
        console.log(jwtToken);
    }
    return new Promise(function(resolve, reject) {
    $.ajax({
        type: type,
        url: url,
        data: data,
        //cors: true,
        //contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        // secure: true,
        headers: {
            'Access-Control-Allow-Credentials' : true,  
            'Access-Control-Allow-Origin': '*',  
            'Access-Control-Allow-Methods':'*',  
            'Access-Control-Allow-Headers':'application/json',
            "Content-Type": "application/json",
            "Authorization" : jwtToken,
        },
        dataType: 'json',
        //jsonp: 'jsonp-callback',
        crossDomain: true,

        beforeSend:function(){
            $("#loadingAnimation").removeClass("d-none");
        },

        success: function(result){
           resolve(result);
        },
        error: function(error, textStatus, errorThrown){
            console.log(textStatus);
            console.log(errorThrown);
            reject(error);
        },

        complete: function(){
            $("#loadingAnimation").addClass("d-none");
        }

      });
    });
}

function addBookAjax(type, url, data){
    //get token from cookie    
    let cookie = document.cookie.split("=")[1];
    let cookieObj = "";
    if(isLoggedin()){
        let cookieObj = JSON.parse(cookie);
        console.log(cookieObj);
        jwtToken ="Bearer " + cookieObj.token;
        console.log(jwtToken);
    }
    return new Promise(function(resolve, reject) {
    $.ajax({
        type: type,
        url: url,
        data: data,
        processData: false,
        contentType: false,
        //cors: true,
        //contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        // secure: true,
        headers: {
            'Access-Control-Allow-Credentials' : true,  
            'Access-Control-Allow-Origin': '*',  
            'Access-Control-Allow-Methods':'*',
            'Access-Control-Allow-Headers':'application/json',
            "Authorization" : jwtToken,
        },
        //jsonp: 'jsonp-callback',
        beforeSend:function(){
            $("#loadingAnimation").removeClass("d-none");
        },
        crossDomain: true,
        success: function(result){
           resolve(result);
        },
        error: function(error, textStatus, errorThrown){
            console.log(textStatus);
            console.log(errorThrown);
            reject(error);
        },
        
        complete: function(){
            $("#loadingAnimation").addClass("d-none");
        }
      });
    });
}


// document ready function
$(document).ready(function(){
    
    if(isLoggedin()){
        $("#loginLi").addClass("d-none");
        $("#logoutLi").removeClass("d-none");
        $("#registerLi").addClass("d-none");
        $("#myProfileBtn").removeClass("d-none");
    }
    else{
       $("#myProfileBtn").addClass("d-none");
        $("#loginLi").removeClass("d-none");
        $("#logoutLi").addClass("d-none");
        $("#registerLi").removeClass("d-none");   
    }
});

// user is login or not
function isLoggedin(){
    jwtToken =document.cookie.split("=")[0]; 
    console.log(jwtToken);
    if(jwtToken == ""){
        return false;
    }
    else{
        return true;
    }
}

function html_table_to_excel(type, id) {
    var data = document.getElementById(id);

    var file = XLSX.utils.table_to_book(data, { sheet: "sheet1" });

    XLSX.write(file, { bookType: type, bookSST: true, type: 'base64' });

    XLSX.writeFile(file, 'file.' + type);
}

