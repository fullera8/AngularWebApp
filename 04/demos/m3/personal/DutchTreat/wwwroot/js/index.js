console.log("A dutch hello.");

//actions to take after the form is loaded
function onLoad() {
	//hide the form
	var theForm = document.getElementById("formDiv");
	theForm.hidden = true;

	//add buy button functionality
	var button = document.getElementById("buyButton");
    button.addEventListener("click", function() {
		console.log("Buying Item");
	});

	//
}


//detect IE and version number through injected conditional comments (no UA detect, no need for cond. compilation / jscript check)
//version arg is for IE version (optional)
//comparison arg supports 'lte', 'gte', etc (optional)
function isIE(version, comparison) {
	var cc = 'IE',
		b = document.createElement('B'),
		docElem = document.documentElement,
		isIE;

	if (version) {
		cc += ' ' + version;
		if (comparison) { cc = comparison + ' ' + cc; }
	}

	b.innerHTML = '<!--[if ' + cc + ']><b id="iecctest"></b><![endif]-->';
	docElem.appendChild(b);
	isIE = !!document.getElementById('iecctest');
	docElem.removeChild(b);
	return isIE;
}

//equivlent of $(document).ready(). IE 6-8 are not compatible with DOMContentLoaded object and need a unique check
if (isIE(6) || isIE(7) || isIE(8)) {
	document.attachEvent("onreadystatechange", function () {
		if (document.readyState === "complete") {
			onLoad();
		}
	});
}
//All other browsers can use this for document ready
else {
	document.addEventListener("DOMContentLoaded", function (event) {
		onLoad();
	});
}



