$("#register").click(addUser);


function addUser(){

    let data = {};
    data.firstName =$.trim($("#firstName").val());
    data.lastName = $.trim($("#lastName").val());
    data.email = $.trim($("#email").val());
    data.roleId = $.trim($("#role").val());
    data.password =$.trim($("#password").val());
    
    let confirmPwd = $.trim($("#confirmPassword").val());

    if(data.firstName == ""){
        $("#registrationAlert").addClass("alert-danger").removeClass("alert-success d-none").text("Firstname is required");
        $("#firstName").focus();
    }
    else if(data.lastName == ""){
        
        $("#registrationAlert").addClass("alert-danger").removeClass("alert-success d-none").text("Lastname is required");
        $("#lastName").focus();
    }
    else if(data.email == ""){
        
        $("#registrationAlert").addClass("alert-danger").removeClass("alert-success d-none").text("Email is required");
        $("#email").focus();   
    }
    else if(data.password == ""){
        
        $("#registrationAlert").addClass("alert-danger").removeClass("alert-success d-none").text("Password is required");
        $("#password").focus();

    }
    else if(confirmPwd == ""){
        
        $("#registrationAlert").addClass("alert-danger").removeClass("alert-success d-none").text("Confirm Password is required");
        $("#confirmPassword").focus();
    }
    else if(data.password != confirmPwd){
        
        $("#registrationAlert").addClass("alert-danger").removeClass("alert-success d-none").text("Password and Confirm Password must be same");
        $("#confirmPassword").focus();
    }
    else{

        callAjax("Post", "https://localhost:44325/api/BookStore/RegisterUser", JSON.stringify(data)).then(function(result){
            console.log(result);
            if(result.code == "OK"){
                $("#reset").click();
                $("#registrationAlert").removeClass("d-none alert-danger").addClass("alert-success").text("You are registered successfully").fadeIn().fadeOut(7000);
            }
        }).catch(function(error){
            console.log(error);
            
            console.log(error.responseJSON.detail);
            $("#registrationAlert").removeClass("d-none alert-success").addClass("alert-danger").text(error.responseJSON.detail).fadeIn().fadeOut(7000);
        });

    }
}