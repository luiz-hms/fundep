function showGlobalLoader() {
    var el = document.getElementById("globalLoader");
    if (el) el.style.display = "flex";
}

function hideGlobalLoader() {
    var el = document.getElementById("globalLoader");
    if (el) el.style.display = "none";
}

document.addEventListener("DOMContentLoaded", function () {
    showGlobalLoader();
    window.addEventListener("load", function () {
        setTimeout(hideGlobalLoader, 250);
    });
});

(function attachAspNetAjaxLoader() {
    if (typeof Sys === "undefined" || !Sys.WebForms) return;

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (!prm) return;

    prm.add_initializeRequest(function () {
        showGlobalLoader();
    });

    prm.add_endRequest(function () {
        setTimeout(hideGlobalLoader, 150);
    });
})();

function onlyNumbers(e) {
    var key = e.keyCode || e.which;

    if (key === 8 || key === 9 || key === 13 || key === 46) return true;

    if (key >= 48 && key <= 57) return true;

    return false;
}

function togglePassword(aspNetId) {
    var input = document.getElementById(aspNetId);

    if (!input) input = document.querySelector('[id$="' + aspNetId + '"]');
    if (!input) return;

    input.type = input.type === "password" ? "text" : "password";
}

function onWebFormsReady(fn) {
    document.addEventListener("DOMContentLoaded", fn);

    if (typeof Sys !== "undefined" && Sys.WebForms) {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm) prm.add_endRequest(fn);
    }
}
function setupProjectSearchToggle() {
    if (!window.projectSearchIds) return;

    var txtNumber = document.getElementById(window.projectSearchIds.number);
    var txtName = document.getElementById(window.projectSearchIds.name);
    var btnSearch = document.getElementById(window.projectSearchIds.btnSearch);

    if (!txtNumber || !txtName || !btnSearch) return;

    function update() {
        var hasNumber = txtNumber.value.trim().length > 0;
        var hasName = txtName.value.trim().length > 0;
        btnSearch.disabled = !(hasNumber || hasName);
    }

    txtNumber.oninput = update;
    txtName.oninput = update;

    update();
}

onWebFormsReady(setupProjectSearchToggle);
function setupCoordinatorSearchToggle() {
    if (!window.coordSearchIds) return;

    var txt = document.getElementById(window.coordSearchIds.text);
    var btnSearch = document.getElementById(window.coordSearchIds.btnSearch);

    if (!txt || !btnSearch) return;

    function update() {
        btnSearch.disabled = (txt.value.trim().length === 0);
    }

    txt.oninput = update;

    update();
}

onWebFormsReady(setupCoordinatorSearchToggle);
