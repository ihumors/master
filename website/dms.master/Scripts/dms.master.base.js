//extend object
Object.extend = function (destination, source) {
    for (var property in source) {
        destination[property] = source[property];
    }
    return destination;
}
// define a method[create] to new a object/class.
var Class = {
    create: function () {
        return function () { this.initialize.apply(this, arguments); }
    }
}
// define the StringBuilder object
if (typeof StringBuilder == "undefined" || !window.StringBuilder) {
    var StringBuilder = function () {
        this.cache = [];
        if (arguments.length) this.append.apply(this, arguments);
    }
}
// add prototype
StringBuilder.prototype = {
    prepend: function () {
        this.cache.splice.apply(this.cache, [].concat.apply([0, 0], arguments));
        return this;
    },
    append: function () {
        this.cache = this.cache.concat.apply(this.cache, arguments);
        return this;
    },
    toString: function () {
        return this.getString();
    },
    getString: function () {
        return this.cache.join('');
    }
};
// define the dms object
if (typeof dms == "undefined" || !window.dms) {
    var dms = {} || window.dms;
};
// browser object
if (typeof sys == "undefined" || !window.sys) {
    var sys = {} || window.sys;
    sys.browser = {} || window.sys;
}
(function () {
    // browsers enum
    if (sys.browser.platform) return;
    var ua = window.navigator.userAgent;
    sys.browser.platform = window.navigator.platform;
    sys.browser.firefox = ua.indexOf("Firefox") > 0;
    sys.browser.chrome = ua.indexOf("Chrome") > 0;
    sys.browser.opera = typeof (window.opera) == "object";
    sys.browser.ie = !sys.browser.opera && ua.indexOf("MSIE") > 0;
    sys.browser.mozilla = window.navigator.product == "Gecko";
    sys.browser.netscape = window.navigator.vendor == "Netscape";
    sys.browser.gecko = ua.indexOf('Gecko') > -1 && ua.indexOf('KHTML') == -1;
    sys.browser.safari = ua.indexOf("Safari") > -1 && !sys.browser.chrome;
    // browser version
    if (sys.browser.firefox) var re = /Firefox(\s|\/)(\d+(\.\d+)?)/;
    else if (sys.browser.ie) var re = /MSIE( )(\d+(\.\d+)?)/;
    else if (sys.browser.opera) var re = /Opera(\s|\/)(\d+(\.\d+)?)/;
    else if (sys.browser.netscape) var re = /Netscape(\s|\/)(\d+(\.\d+)?)/;
    else if (sys.browser.safari) var re = /Version(\/)(\d+(\.\d+)?)/;
    else if (sys.browser.mozilla) var re = /rv(\:)(\d+(\.\d+)?)/;
    else if (sys.browser.chrome) var re = /Chrome(\/)(\d+(\.\w)?)/;
    if ("undefined" != typeof (re) && re.test(ua))
        sys.browser.version = parseFloat(RegExp.$2);
})();
// extend a prototype in the dms object
Object.extend(dms, {
    host: "/"
});

//define dms.partial object and the base features implement
dms.partial = {
    setting: {
        paging: {
            ajaxHanlder: null,
            ajaxUrl: null,
            ajaxData: {
                totalCount: 1,
                pageIndex: 1,
                pageSize: 15,
                sort: "",
                sortby: "",
                keys: ""
            },
            breadCrumbContainer: null,
            listContainer: null,
            pagingContainer: null,
            paingInnerHtml: "",
            loadingContainer: null
        },
        upfile: {
            imgobj: "#nat-imgurl",
            maxSize: 50,
            option: {
                type: "POST",
                dataType: "json",
                success: function (json) {
                    if (!json.Success) {
                        return alert(json.Message);
                    }
                    $(dms.partial.setting.upfile.imgobj).attr("src", json.Message);
                },
                error: function () {
                    alert("上传图片出错");
                }
            }
        }
    },
    init: function () {
        this.initPaging();
        this.bindResize();
        this.bindPopZindex();
    },
    initPaging: function () {
        $("body").delegate(dms.partial.setting.paging.pagingContainer + " li a", "click", function () {
            var index = $(this).attr("data-index");
            if (index == undefined || index == "") return;
            dms.partial.go(index);
            return false;
        });
    },
    bindPopZindex: function () {
        $(document).delegate(".dms-popwin", "mousedown", function () {
            $(this).css("z-index", ++dms.win.setting.zindex);
        });
    },
    bindResize: function () {
        dms.partial.resize();
        $(window).resize(function () {
            dms.partial.resize();
        });
    },
    resize: function () {
        var header = $("header").first().height(), footer = $("footer").first().height();
        $(".body-content").height($(window).height() - header - footer - 4);
    },
    //paging:via page index
    go: function (index) {
        $.extend(true, dms.partial.setting.paging.ajaxData, { pageIndex: index });
        dms.partial.loadDataPartial();
    },
    refresh: function () {
        dms.partial.go(dms.partial.setting.paging.ajaxData.pageIndex);
    },
    //load data list to partail 
    loadDataPartial: function (callback) {
        var $paging = dms.partial.setting.paging;
        if ($paging.ajaxHanlder != null) {
            dms.partial.setting.paging.ajaxHanlder.abort();
            dms.partial.setting.paging.ajaxHanlder = null;
        }

        dms.partial.setting.paging.ajaxHanlder = $.ajax({
            url: $paging.ajaxUrl,
            type: "POST",
            data: $paging.ajaxData,
            beforeSend: function () {
                $($paging.loadingContainer).show();
            },
            complete: function () {
                $($paging.loadingContainer).fadeOut();
            },
            success: function (jsonData) {
                if (jsonData.bizCode != 200) {
                    $($paging.listContainer).html(jsonData.message);
                    return false;
                }

                dms.partial.setting.paging.paingInnerHtml = jsonData.result.pagination;
                $($paging.listContainer).html(jsonData.result.datalist);
                //$($paging.breadCrumbContainer).html(result.breadCrumb)

                if (callback != null && typeof callback == "function") {
                    callback();
                }

                dms.partial.resize();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //alert(errorThrown);
            }
        });
    },
    initUpfile: function (imgobj, upfilebutton, upfileForm) {
        dms.partial.setting.upfile.imgobj = imgobj;
        $("body").delegate(upfilebutton, "change", function () {
            if ($(this).val() !== "") {
                var file_size = this.files[0].size;
                var size = file_size / 1024;
                if (size > dms.partial.setting.upfile.maxSize) {
                    alert("上传的图片大小不能超过" + (dms.partial.setting.upfile.maxSize > 1024 ? (dms.partial.setting.upfile.maxSize / 1024) + "M" : dms.partial.setting.upfile.maxSize + "K") + "B！" + ",且必须是图片格式");
                    $(this).val("");
                    return false;
                }
                var imgobj = $(upfilebutton).attr("data-imageid");
                if (imgobj != undefined && imgobj != "" && imgobj != null)
                    dms.partial.setting.upfile.imgobj = "#" + imgobj;

                $(upfileForm).ajaxSubmit(dms.partial.setting.upfile.option);
            }
        });
    },
}

// popup win
dms.get = {
    byId: function (id) {
        return typeof id === "string" ? document.getElementById(id) : id;
    },
    byClass: function (sClass, oParent) {
        var aClass = [];
        var reClass = new RegExp("(^| )" + sClass + "( |$)");
        var aElem = this.byTagName("*", oParent);
        for (var i = 0; i < aElem.length; i++) reClass.test(aElem[i].className) && aClass.push(aElem[i]);
        return aClass;
    },
    byTagName: function (elem, obj) {
        return (obj || document).getElementsByTagName(elem);
    }
};
dms.win = {
    setting: {
        async: null,
        zindex: 1,
        width: 650,
        height: 300
    },
    init: function (oDrag, handle) {
        var disX = dixY = 0;
        var oMin = dms.get.byClass("popwin-min", oDrag)[0];
        var oMax = dms.get.byClass("popwin-max", oDrag)[0];
        var oRevert = dms.get.byClass("popwin-revert", oDrag)[0];
        var oClose = dms.get.byClass("popwin-close", oDrag)[0];
        handle = handle || oDrag;
        handle.style.cursor = "move";
        handle.onmousedown = function (event) {
            var event = event || window.event;
            disX = event.clientX - oDrag.offsetLeft;
            disY = event.clientY - oDrag.offsetTop;

            document.onmousemove = function (event) {
                var event = event || window.event;
                var iL = event.clientX - disX;
                var iT = event.clientY - disY;
                var maxL = document.documentElement.clientWidth - oDrag.offsetWidth;
                var maxT = document.documentElement.clientHeight - oDrag.offsetHeight;

                iL <= 0 && (iL = 0);
                iT <= 0 && (iT = 0);
                iL >= maxL && (iL = maxL);
                iT >= maxT && (iT = maxT);

                oDrag.style.left = iL + "px";
                oDrag.style.top = iT + "px";

                return false
            };

            document.onmouseup = function () {
                document.onmousemove = null;
                document.onmouseup = null;
                this.releaseCapture && this.releaseCapture()
            };
            this.setCapture && this.setCapture();
            return false
        };
        //最大化按钮
        oMax.onclick = function () {
            oDrag.style.top = oDrag.style.left = "1px";
            oDrag.style.width = document.documentElement.clientWidth - 4 + "px";
            oDrag.style.height = document.documentElement.clientHeight - 4 + "px";
            this.style.display = "none";
            oRevert.style.display = "block";
            dms.win.calcnth(oDrag);
        };
        //还原按钮
        oRevert.onclick = function () {
            var clientWidth = document.documentElement.clientWidth, clientHeight = document.documentElement.clientHeight;
            oDrag.style.width = dms.win.setting.width > clientWidth ? clientWidth : dms.win.setting.width + "px";
            oDrag.style.height = dms.win.setting.height > clientHeight ? clientHeight : dms.win.setting.height + "px";
            oDrag.style.left = (document.documentElement.clientWidth - oDrag.offsetWidth) / 2 + "px";
            oDrag.style.top = (document.documentElement.clientHeight - oDrag.offsetHeight) / 2 + "px";
            this.style.display = "none";
            oMax.style.display = "block";
            dms.win.calcnth(oDrag);
        };
        //最小化按钮
        oMin.onclick = function () {
            oDrag.style.display = "none";
            var oA = document.createElement("a");
            oA.className = "open-dms-win";
            oA.id = oDrag.id + "-min";
            oA.href = "javascript:;";
            oA.title = "还原";
            document.body.appendChild(oA);
            oA.onclick = function () {
                oDrag.style.display = "block";
                document.body.removeChild(this);
                this.onclick = null;
            };
        };
        oClose.onclick = function () {
            var top = $(window).height() / 2;
            $(oDrag).animate({
                "top": top + "px",
                "height": "0px",
                "width": "0px",
                "left": "50%"
            }, {
                queue: false,
                duration: 80,
                easing: "easeOutBounce",
                step: function () {
                },
                complete: function () {
                    $(oDrag).css({ "visibility": "hidden" });
                    $("#" + oDrag.id + " .dms-popwin-content").html("玩命加载中...");
                    $(window).trigger("resize");
                }
            });
        };
        //阻止冒泡
        oMin.onmousedown = oMax.onmousedown = oClose.onmousedown = function (event) {
            this.onfocus = function () { this.blur() };
            (event || window.event).cancelBubble = true
        };
    },
    mousedown: function (oParent, handle, isLeft, isTop, lockX, lockY) {
        handle.onmousedown = function (event) {
            var event = event || window.event;
            var disX = event.clientX - handle.offsetLeft;
            var disY = event.clientY - handle.offsetTop;
            var iParentTop = oParent.offsetTop;
            var iParentLeft = oParent.offsetLeft;
            var iParentWidth = oParent.offsetWidth;
            var iParentHeight = oParent.offsetHeight;
            
            document.onmousemove = function (event) {
                var event = event || window.event;
                var dragMinWidth = dms.win.setting.width;
                var dragMinHeight = dms.win.setting.height;
                var iL = event.clientX - disX;
                var iT = event.clientY - disY;
                var maxW = document.documentElement.clientWidth - oParent.offsetLeft - 2;
                var maxH = document.documentElement.clientHeight - oParent.offsetTop - 2;
                var iW = isLeft ? iParentWidth - iL : handle.offsetWidth + iL;
                var iH = isTop ? iParentHeight - iT : handle.offsetHeight + iT;

                isLeft && (oParent.style.left = iParentLeft + iL + "px");
                isTop && (oParent.style.top = iParentTop + iT + "px");

                iW < dragMinWidth && (iW = dragMinWidth);
                iW > maxW && (iW = maxW);
                lockX || (oParent.style.width = iW + "px");

                iH < dragMinHeight && (iH = dragMinHeight);
                iH > maxH && (iH = maxH);
                lockY || (oParent.style.height = iH + "px");

                if ((isLeft && iW == dragMinWidth) || (isTop && iH == dragMinHeight)) document.onmousemove = null;

                dms.win.calcnth(oParent);
                return false;
            };
            document.onmouseup = function () {
                document.onmousemove = null;
                document.onmouseup = null;
            };
            return false;
        }
    },
    resize: function () {
        var win = $(window), winHeight = win.height(), winWidth = win.width();
        var popWidth = winWidth - 4, popHeight = winHeight - 6, offTop = 1, offLeft = 1;
        // if the popwin is show, then set the relate css property value.
        var $popwin = $(".dms-popwin");
        if ($popwin != null && $popwin.size() > 0) {
            $.each($popwin, function (index, item) {
                var popwin = $(item);
                if (popwin.css("visibility") == "visible") {
                    if (!$(".max").is(":hidden")) {
                        popWidth = popWidth > dms.win.setting.width ? dms.win.setting.width : popWidth;
                        popHeight = popHeight > dms.win.setting.height ? dms.win.setting.height : popHeight;
                        offTop = (winHeight - popHeight) / 2;
                        offLeft = (winWidth - popWidth) / 2;
                        popWidth = popWidth - 2;
                        popHeight = popHeight - 2;
                    }
                    popwin.css({
                        "top": offTop + "px",
                        "left": offLeft + "px",
                        "height": popHeight + "px",
                        "width": popWidth + "px"
                    });
                    var $id = "#" + popwin.attr("id");
                    $($id + " .dms-popwin-content").height(popHeight - $($id + " .dms-popwin-title").height() - $($id + " .dms-popwin-bottom").height() - 6);
                }
            });
        }
    },
    open: function (url, id, data, callback) {
        typeof id === "string" ? id : "dms-popwin";
        var oDrag = dms.get.byId(id);
        if ($("#" + id).attr("data-init") != "true") {
            dms.win.init(oDrag, dms.get.byClass("dms-popwin-title", oDrag)[0]);
            dms.win.mousedown(oDrag, dms.get.byClass("resizeLT", oDrag)[0], true, true, false, false);
            dms.win.mousedown(oDrag, dms.get.byClass("resizeTR", oDrag)[0], false, true, false, false);
            dms.win.mousedown(oDrag, dms.get.byClass("resizeBR", oDrag)[0], false, false, false, false);
            dms.win.mousedown(oDrag, dms.get.byClass("resizeLB", oDrag)[0], true, false, false, false);
            dms.win.mousedown(oDrag, dms.get.byClass("resizeL", oDrag)[0], true, false, false, true);
            dms.win.mousedown(oDrag, dms.get.byClass("resizeT", oDrag)[0], false, true, true, false);
            dms.win.mousedown(oDrag, dms.get.byClass("resizeR", oDrag)[0], false, false, false, true);
            dms.win.mousedown(oDrag, dms.get.byClass("resizeB", oDrag)[0], false, false, true, false);
            $("#" + id).css("z-index", ++dms.win.setting.zindex);
        }
        if (oDrag.style.visibility != "visible") {
            var popwin = $("#" + id);
            var win = $(window), winHeight = win.height(), winWidth = win.width();
            var doc = $(document), docHeight = doc.height(), docWidth = doc.width();
            // set the pop win css property.
            var popWidth = dms.win.setting.width, popHeight = winHeight;
            popHeight = popHeight > dms.win.setting.height ? dms.win.setting.height : popHeight;

            var offTop = 0, offLeft = 0;

            offTop = (winHeight - popHeight) / 2;
            offLeft = (winWidth - popWidth) / 2;

            popwin.show().css("visibility", "visible").animate({
                left: offLeft + 'px',
                top: offTop + "px",
                width: popWidth + 'px',
                height: popHeight + "px"
            }, {
                queue: false,
                duration: 50,
                easing: "easeInCubic",
                step: function () {
                },
                complete: function () {
                    $("#" + id).attr("data-init", "true");
                    $(window).trigger("resize");
                    dms.win.calcnth(oDrag);
                }
            });
        } else {
            $("#" + id).css("z-index", ++dms.win.setting.zindex);
        }
        $("#" + id).attr("data-url", url);
        if (typeof data != "undefined") {
            $("#" + id).attr("data-argument", typeof data === "string" ? data : JSON.stringify(data));
        }
        if (dms.win.setting.async != null) {
            dms.win.setting.async.abort();
            dms.win.setting.async = null;
        }
        dms.win.setting.async = $.ajax({
            url: url,
            type: "POST",
            async: true,
            data: data,
            beforeSend: function () {
                $("#" + id + " .dms-popwin-title-label").html("loading...");
            },
            complete: function () { },
            success: function (html) {
                $("#" + id + " .dms-popwin-content").html(html);
                dms.partial.resize();
                if (typeof callback == "function") { callback(); }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("#" + id + " .dms-popwin-progress").html("<font color='red'>加载失败了,错误信息：" + errorThrown + "</font>");
            }
        });
    },
    submit: function (args) {

    },
    cancel: function ($this) {
        var id = $($this).attr("data-id"), oWin = $("#" + id)
        dms.win.open(oWin.attr("data-url"), id, eval("(" + oWin.attr("data-argument") + ")"));
    },
    calcnth: function (oParent) {
        var $id = "#" + oParent.id;
        $($id + " .dms-popwin-content").height($($id).height() - $($id + " .dms-popwin-title").height() - $($id + " .dms-popwin-bottom").height() - 6);
    }
};

// jQuery Easing Define.
jQuery.easing['jswing'] = jQuery.easing['swing'];
jQuery.extend(jQuery.easing,
{
    def: 'easeOutQuad',
    swing: function (x, t, b, c, d) {
        //alert(jQuery.easing.default);
        return jQuery.easing[jQuery.easing.def](x, t, b, c, d);
    },
    easeInQuad: function (x, t, b, c, d) {
        return c * (t /= d) * t + b;
    },
    easeOutQuad: function (x, t, b, c, d) {
        return -c * (t /= d) * (t - 2) + b;
    },
    easeInOutQuad: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t + b;
        return -c / 2 * ((--t) * (t - 2) - 1) + b;
    },
    easeInCubic: function (x, t, b, c, d) {
        return c * (t /= d) * t * t + b;
    },
    easeOutCubic: function (x, t, b, c, d) {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    },
    easeInOutCubic: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t + 2) + b;
    },
    easeInQuart: function (x, t, b, c, d) {
        return c * (t /= d) * t * t * t + b;
    },
    easeOutQuart: function (x, t, b, c, d) {
        return -c * ((t = t / d - 1) * t * t * t - 1) + b;
    },
    easeInOutQuart: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
        return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
    },
    easeInQuint: function (x, t, b, c, d) {
        return c * (t /= d) * t * t * t * t + b;
    },
    easeOutQuint: function (x, t, b, c, d) {
        return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
    },
    easeInOutQuint: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
    },
    easeInSine: function (x, t, b, c, d) {
        return -c * Math.cos(t / d * (Math.PI / 2)) + c + b;
    },
    easeOutSine: function (x, t, b, c, d) {
        return c * Math.sin(t / d * (Math.PI / 2)) + b;
    },
    easeInOutSine: function (x, t, b, c, d) {
        return -c / 2 * (Math.cos(Math.PI * t / d) - 1) + b;
    },
    easeInExpo: function (x, t, b, c, d) {
        return (t == 0) ? b : c * Math.pow(2, 10 * (t / d - 1)) + b;
    },
    easeOutExpo: function (x, t, b, c, d) {
        return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b;
    },
    easeInOutExpo: function (x, t, b, c, d) {
        if (t == 0) return b;
        if (t == d) return b + c;
        if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b;
        return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b;
    },
    easeInCirc: function (x, t, b, c, d) {
        return -c * (Math.sqrt(1 - (t /= d) * t) - 1) + b;
    },
    easeOutCirc: function (x, t, b, c, d) {
        return c * Math.sqrt(1 - (t = t / d - 1) * t) + b;
    },
    easeInOutCirc: function (x, t, b, c, d) {
        if ((t /= d / 2) < 1) return -c / 2 * (Math.sqrt(1 - t * t) - 1) + b;
        return c / 2 * (Math.sqrt(1 - (t -= 2) * t) + 1) + b;
    },
    easeInElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        return -(a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
    },
    easeOutElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3;
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        return a * Math.pow(2, -10 * t) * Math.sin((t * d - s) * (2 * Math.PI) / p) + c + b;
    },
    easeInOutElastic: function (x, t, b, c, d) {
        var s = 1.70158; var p = 0; var a = c;
        if (t == 0) return b; if ((t /= d / 2) == 2) return b + c; if (!p) p = d * (.3 * 1.5);
        if (a < Math.abs(c)) { a = c; var s = p / 4; }
        else var s = p / (2 * Math.PI) * Math.asin(c / a);
        if (t < 1) return -.5 * (a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b;
        return a * Math.pow(2, -10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b;
    },
    easeInBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        return c * (t /= d) * t * ((s + 1) * t - s) + b;
    },
    easeOutBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
    },
    easeInOutBack: function (x, t, b, c, d, s) {
        if (s == undefined) s = 1.70158;
        if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
        return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
    },
    easeInBounce: function (x, t, b, c, d) {
        return c - jQuery.easing.easeOutBounce(x, d - t, 0, c, d) + b;
    },
    easeOutBounce: function (x, t, b, c, d) {
        if ((t /= d) < (1 / 2.75)) {
            return c * (7.5625 * t * t) + b;
        } else if (t < (2 / 2.75)) {
            return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
        } else if (t < (2.5 / 2.75)) {
            return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
        } else {
            return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
        }
    },
    easeInOutBounce: function (x, t, b, c, d) {
        if (t < d / 2) return jQuery.easing.easeInBounce(x, t * 2, 0, c, d) * .5 + b;
        return jQuery.easing.easeOutBounce(x, t * 2 - d, 0, c, d) * .5 + c * .5 + b;
    }
});

// 序列化表单元素到json对象
$.fn.serializeJson = function () {
        var serializeObj = {};
        var array = this.serializeArray();
        var str = this.serialize();
        $(array).each(function () {
            if (serializeObj[this.name]) {
                if ($.isArray(serializeObj[this.name])) {
                    serializeObj[this.name].push(this.value);
                } else {
                    serializeObj[this.name] = [serializeObj[this.name], this.value];
                }
            } else {
                serializeObj[this.name] = this.value;
            }
        });
        return serializeObj;
    };
// h+plus
dms.hlus = {
    t: function (t) {
        var e = 0;
        return top.$(t).each(function () {
            e += top.$(this).outerWidth(!0)
        }),
        e
    },
    e: function (e) {
        var a = this.t(top.$(e).prevAll()),
        i = this.t(top.$(e).nextAll()),
        n = this.t(top.$(".content-tabs").children().not(".J_menuTabs")),
        s = top.$(".content-tabs").outerWidth(!0) - n,
        r = 0;
        if (top.$(".page-tabs-content").outerWidth() < s) r = 0;
        else if (i <= s - top.$(e).outerWidth(!0) - top.$(e).next().outerWidth(!0)) {
            if (s - top.$(e).next().outerWidth(!0) > i) {
                r = a;
                for (var o = e; r - top.$(o).outerWidth() > top.$(".page-tabs-content").outerWidth() - s;) r -= top.$(o).prev().outerWidth(),
                o = $(o).prev()
            }
        } else a > s - top.$(e).outerWidth(!0) - top.$(e).prev().outerWidth(!0) && (r = a - top.$(e).prev().outerWidth(!0));
        top.$(".page-tabs-content").animate({
            marginLeft: 0 - r + "px"
        },
        "fast");
    },
    n: function ($this) {
        var _this = $this;
        var t = $(_this).data("href"),
        i = $.trim($(_this).text()),
        n = !0;
        if (void 0 == t || 0 == top.$.trim(t).length) return !1;
        if (top.$(".J_menuTab").each(function (index,element) {
            return $(element).data("id") == t ? ($(element).hasClass("active") || ($(element).addClass("active").siblings(".J_menuTab").removeClass("active"),
            dms.hlus.e(element),
            top.$(".J_mainContent .J_iframe").each(function (index,element) {
                return $(element).data("id") == t ? ($(element).show().siblings(".J_iframe").hide(), !1) : void 0
        })), n = !1, !1) : void 0
        }), n) {
            var s = '<a href="javascript:;" class="active J_menuTab" data-id="' + t + '">' + i + ' <i class="fa fa-times-circle"></i></a>';
            top.$(".J_menuTab").removeClass("active");
            var r = '<iframe class="J_iframe" name="iframe' + a + '" width="100%" height="100%" src="' + t + '?v=4.0" frameborder="0" data-id="' + t + '" seamless></iframe>';
            top.$(".J_mainContent").find("iframe.J_iframe").hide().parents(".J_mainContent").append(r);
            top.$(".J_menuTabs .page-tabs-content").append(s),
            this.e(top.$(".J_menuTab.active"))
        }
        return !1
    }
};
// toolbar
dms.tool = {
    add: function () {
        // ext to do
    },
    delete: function () {
        // ext to do
    },
    refresh: function () {
        dms.partial.refresh();
    }
}
dms.partial.init();