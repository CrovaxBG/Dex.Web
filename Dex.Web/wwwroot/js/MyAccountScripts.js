$(document).ready(function ($) {
    //globalData = document.querySelector('#model-data');
    currentUserId = $('#CurrentUserId').val();
    selectedUserId = $('#SelectedUserId').val();
});

$('#privilegesTableDiv').on('click',
    'span.removePrivilege',
    function() {
        var row = this;
        var privilegeData = getPrivilegeData(row);
        row.remove();
        removePrivilege(window.currentUserId, privilegeData.type, privilegeData.value)
            .done(function() {
                refreshAccountSignIn(window.currentUserId);
            });
    });

$('#projectFavoritesTableDiv').on('click',
    'span.removeProjectFavorite',
    function() {
        var row = this;
        var favoriteData = getProjectFavoriteData(row);
        row.remove();
        removeProjectFavorite(window.currentUserId, favoriteData.id)
            .done(function () {
                updateProjectFavoritesTable(window.currentUserId);
            });
    });

$('#addPrivilegeForm').on('submit',
    function(event) {
        event.preventDefault();
        if ($(this).valid()) {
            var privilegeType = $('#privilegeType').val();
            var privilegeValue = $('#privilegeValue').val();
            addPrivilege(window.currentUserId, privilegeType, privilegeValue)
                .fail(function(jqXHR) {
                    if (jqXHR.status === 400) {
                        $.growl.error({ message: 'Invalid data! Could not add privilege.' });
                    } else if (jqXHR.status === 409) {
                        $.growl.error({ message: 'Duplicate data! Could not add privilege.' });
                    }
                })
                .done(function() {
                    refreshAccountSignIn(window.currentUserId);
                });
        }
    });

$('#privilegeType').on('change',
    function () {
        var val = $('#privilegeType').val();

        $.ajax({
            type: 'GET',
            url: '/Account/GetCurrentUserClaimsByType',
            accepts: 'text/json',
            data: {
                type: val
            },
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            }
        }).done(function (response) {
            var el = $('#privilegeValue');
            el.empty();
            $.each(response,
                function (key, value) {
                    el.append($("<option></option>")
                        .attr("value", value.type).text(value.value));
                });
        });
    });

function updatePrivilegesTable(userId) {
    return getCurrentUserPrivileges(userId)
        .done(function (data) {
            getPrivilegesPartialTable(data)
                .done(function (response) {
                    var previousScroll = $('#privilegesTableScroll').scrollTop();
                    var previousHeight = $('#privilegesTable').height();
                    $('#privilegesTableDiv').html(response);

                    if (previousHeight !== null) {
                        var sizeDifference = previousHeight - $('#privilegesTable').height();
                        $('#privilegesTableScroll').scrollTop(previousScroll - sizeDifference);
                    }
                });
        });
}

function updateProjectFavoritesTable(userId) {
    return getCurrentUserProjectFavorites(userId)
        .done(function (data) {
            getProjectFavoritesPartialTable(data)
                .done(function (response) {
                    var previousScroll = $('#projectFavoritesTableScroll').scrollTop();
                    var previousHeight = $('#projectFavoritesTable').height();
                    $('#projectFavoritesTableDiv').html(response);

                    if (previousHeight !== null) {
                        var sizeDifference = previousHeight - $('#projectFavoritesTable').height();
                        $('#projectFavoritesTableScroll').scrollTop(previousScroll - sizeDifference);
                    }
                });
        });
}

function getPrivilegesPartialTable(data) {
    return $.ajax({
        url: '/Account/GetPrivilegesPartialTable',
        type: 'POST',
        accepts: 'text/json',
        data: {
            claims: data
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        }
    });
}

function getProjectFavoritesPartialTable(data) {
    return $.ajax({
        url: '/Account/GetProjectFavoritesPartialTable',
        type: 'POST',
        accepts: 'text/json',
        data: {
            projectFavorites: data
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        }
    });
}

function addPrivilege(userId, claimType, claimValue) {
    return $.ajax({
        type: "POST",
        url: '/api/UserClaims/AddUserClaim',
        contentType: 'application/json',
        data: JSON.stringify({
            userId: userId,
            claimType: claimType,
            claimValue: claimValue
        }),
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        }
    });
}

function removePrivilege(userId, claimType, claimValue) {
    return $.ajax({
        url: '/api/UserClaims/RemoveUserClaim',
        type: 'DELETE',
        contentType: 'application/json',
        data: JSON.stringify({
            userId: userId,
            claimType: claimType,
            claimValue: claimValue
        }),
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        }
    });
}

function removeProjectFavorite(userId, projectId) {
    return $.ajax({
        url: '/api/ProjectFavorites/RemoveFavorite',
        type: 'DELETE',
        contentType: 'application/json',
        data: JSON.stringify({
            userId: userId,
            projectId: projectId
        }),
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        }
    });
}

function getCurrentUserPrivileges(userId) {
    return $.ajax({
        url: '/api/UserClaims/GetUserClaimsByUserId',
        type: 'GET',
        contentType: 'application/json',
        data: {
            userId: userId
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        }
    });
}

function getCurrentUserProjectFavorites(userId) {
    return $.ajax({
        url: '/api/ProjectFavorites/GetFavoritesByUser',
        type: 'GET',
        contentType: 'application/json',
        data: {
            userId: userId
        },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        }
    });
}

function refreshAccountSignIn(userId) {
    return $.ajax({
            url: '/Account/RefreshSignIn',
            type: 'POST',
            data: {
                userId: userId
            },
            headers: {
                RequestVerificationToken:
                    $('input:hidden[name="__RequestVerificationToken"]').val()
            }
        })
        .done(function() {
            updatePrivilegesTable(window.currentUserId);
            updateProjectFavoritesTable(window.currentUserId);
        });
}

function getPrivilegeData(element) {
    var classes = element.className.split(' ');
    var privilegeData = new Object();
    var classPrefix = 'privilege-';
    for (var i = 0; i < classes.length; i++) {
        if (classes[i].indexOf(classPrefix) >= 0) {
            var dataSliced = classes[i].split('-');
            privilegeData.type = dataSliced[1];
            privilegeData.value = dataSliced[2];
            break;
        }
    }
    return privilegeData;
}

function getProjectFavoriteData(element) {
    var classes = element.className.split(' ');
    var favoriteData = new Object();
    var classPrefix = 'projectFavorite-';
    for (var i = 0; i < classes.length; i++) {
        if (classes[i].indexOf(classPrefix) >= 0) {
            var dataSliced = classes[i].split('-');
            favoriteData.id = dataSliced[1];
            break;
        }
    }
    return favoriteData;
}