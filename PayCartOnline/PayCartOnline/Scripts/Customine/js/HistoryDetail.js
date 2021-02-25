
const SearchOrder = () => {
   
    let startDate = $('#startDate').val();
    let expirationDate = $('#expirationDate').val();
    let typePay = $('#typePay').val();
    let status = $('#status').val();
   
    if (startDate != '' && expirationDate == '' || startDate == '' && expirationDate != '' ) {

        alert('Chưa chọn đủ trường')
        return
    }
    
    console.log(startDate + '' + expirationDate)
    if (startDate != '' && expirationDate != '') {
        if (!(Date.parse(startDate) <= Date.parse(expirationDate))) {
            alert('chọn lại ngày tìm kiếm')
            return;
        }
    }
    
   
    $.ajax({
        url: '/User/HistoryDeal',
        type: 'GET',
        data: {
            startDate: startDate,
            expirationDate: expirationDate,
            typePay: typePay,
            status:status
        },
      
       
      

        success: function (response) {
            $('#content').empty()
            $(response).each(function (i, e) {
                e.Price = e.Price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
                var myDate = new Date(e.Create_At.match(/\d+/)[0] * 1);
                let data = ` <tr>
                                <td>`+e.Code_Order+`</td>
                                <td>`+ e.Phone+` </td>
                                <td>`+ e.Brand+`</td>
                                <td>`+ e.Price +`</td>
                                <td>`+ e.CardType+`</td>
                                <td>VNPAY</td>
                                <td>`+ e.BankCode+`</td>
                                <td>`+ e.Status+`</td>
                                <td>`+ myDate+`</td>
                                <td><a href="/User/DetailsOrder?id=`+ e.Id_order+`"><i class="fa fa-eye" aria-hidden="true"></i></a></td>
                            </tr>`
                $('#content').append(data)
            })
        }
    })

}