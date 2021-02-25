

const Search = () => {
    let id_denomination = $('#id_denomination').val();
    let phone = $('#phone').val();
    let status = $('#phone').val();
    let expiration = $('#expiration').val();
    let startDate = $('#startDate').val();

    $.ajax({
        url: '/Admin/Order',
        type: 'GET',
        data: {
            startDate: startDate,
            expiration: expiration,
            phone: phone,
            status: status,
            id_denomination: id_denomination
        },
        success: function (response) {
            alert(response)
        }
    })
}