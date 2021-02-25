

const submit =() => {
    let id_denomination = $('#id_denomination').val();
    let phone = $('#phone').val();
    let status = $('#status').val();
    let expiration = $('#expiration').val();
    let startDate = $('#startDate').val();

    if (startDate = '' && expiration == '' && status == '' && phone == '' && id_denomination == '') {
        alert('bạn chưa chọn trường tìm kiếm ')
        return;
    }
    if (startDate && expiration  && status  && phone  && id_denomination == '') {
        alert('bạn chưa chọn trường tìm kiếm ')
        console.log('abc')
        return;
    }


    if (phone != '' &&phone.toString().length != 9) {
        alert('nhập lại số phone đúng định dạng')
        return;
    }
    
    if (startDate != '' && expiration == '' || startDate == '' && expiration != '') {

        alert('Chưa chọn đủ trường')
        return
    }

    console.log(startDate + '' + expiration)
    if (startDate != '' && expiration != '') {
        if (!(Date.parse(startDate) <= Date.parse(expiration))) {
            alert('chọn lại ngày tìm kiếm')
            return;
        }
    }

    window.location.assign("https://localhost:44392/Admin/Order?startDate=" + startDate + "&&expiration=" + expiration+
        "&status=" + status + "&phone=" + phone + "&id_denomination=" + id_denomination)
  
      
    }