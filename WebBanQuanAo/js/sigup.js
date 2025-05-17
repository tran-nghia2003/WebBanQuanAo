function KiemTraThongTin(){
	var hoTen=document.getElementById("name");

	if(hoTen.value==""){
		alert("Vui lòng nhập Tên đăng nhập");
		hoTen.focus();
		return false;
	}

	var email=document.getElementById("email");
	re=/\S+@\S+\.\S+/;
	if(email.value==""){
		alert("Bạn chưa nhập Email.");
		email.focus();
		return false;
	}

	else
		if(re.test(email.value)==false){
			alert("Email không đúng định dạng!");
			email.focus();
			return false;
		}
	var pass=document.getElementById("password");
	if(pass.value==""){
		alert("Vui lòng nhập mật khẩu.");
		pass.focus();
		return false;
	}
	var SDT=document.getElementById("number");

	if(hoTen.value==""){
		alert("Vui lòng nhập Số điện thoại");
		SDT.focus();
		return false;
	}


	alert("Đăng ký thành công! Xin chúc mừng.");
	return true;
}