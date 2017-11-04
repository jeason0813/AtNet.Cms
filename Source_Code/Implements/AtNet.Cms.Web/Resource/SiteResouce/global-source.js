﻿//2013-01-05  COPYRIGHT 2012 OPSoft
//
// 修改说明:
//  2013-06-04  10:48   newmin [!]: 修正导航在多级栏目下无法选中的bug
//
//Core
function js() { this.__VERSION__ = '3.1'; this.__WORKPATH__ = ''; this.__Extend_PROTOTYPE__ = true } js.prototype = { __init__: function () { if (this.__Extend_PROTOTYPE__) { this.__extendingJsPrototype__() } var f = document.getElementsByTagName('SCRIPT'); var s = f[f.length - 1]; var g = s.src; this.__WORKPATH__ = g.replace(/(\/)[^/]+$/, '$1'); if (!document.getElementsByClassName) { document.getElementsByClassName = function (a, b) { if (b && !b.nodeName) { b = document.getElementById(b) } var c = (b || document).getElementsByTagName('*'); var d = new RegExp('\\s' + a + '\\s'); var e = []; for (var i = 0, j; j = c[i]; i++) { if (d.test(' ' + j.className + ' ')) e.push(j) } return e } } if (typeof (HTMLElement) != "undefined") { HTMLElement.prototype.contains = function (a) { while (a != null && typeof (a.tagName) != "undefind") { if (a == this) return true; a = a.parentNode } return false } } if (!window.toJson) { window.toJson = function (s) { if (!s) return null; if (window.JSON) { try { return JSON.parse(s) } catch (ex) { } } return eval('(' + s + ')') } } }, __extendingJsPrototype__: function () { String.prototype.len = function (a) { return this.replace(a ? /[\u0391-\uFFE5]/g : /[^x00-xff]/g, "00").length }; Date.prototype.format = function (a) { var o = { "M+": this.getMonth() + 1, "d+": this.getDate(), "h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12, "H+": this.getHours(), "m+": this.getMinutes(), "s+": this.getSeconds(), "q+": Math.floor((this.getMonth() + 3) / 3), "S": this.getMilliseconds() }; var b = { "0": "/u65e5", "1": "/u4e00", "2": "/u4e8c", "3": "/u4e09", "4": "/u56db", "5": "/u4e94", "6": "/u516d" }; if (/(y+)/.test(a)) { a = a.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length)) } if (/(E+)/.test(a)) { a = a.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + b[this.getDay() + ""]) } for (var k in o) { if (new RegExp("(" + k + ")").test(a)) { a = a.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length))) } } return a } }, extend: function (a) { if (a && a instanceof Object) { for (var b in a) { if (this[b] == undefined) { this[b] = a[b] } } } }, dom: { fitHeight: function (e, a) { var b = e.parentNode; var c = e.nextSibling; var d = /;(\s*)height:(.+);/ig; var f = (b == document.body ? Math.max(document.body.clientHeight, document.documentElement.clientHeight) : b.offsetHeight) - e.offsetTop; while (c) { if (c.nodeName[0] != '#') { f -= c.offsetHeight } c = c.nextSibling } f -= a || 0; if (d.test(e.style.cssText)) { e.style.cssText = e.style.cssText.replace(d, '; height:' + f + 'px;') } else { e.style.cssText += 'height:' + f + 'px;' } }, $: function (a, b, c) { var e = a.nodeName ? a : document.getElementById(a || ''); if (!e) throw a.nodeName ? 'object refrence null' : 'element ' + a + ' not exits!'; if (!b) return e; e = e.getElementsByTagName(b); if (!c) return e; var d = []; var f; for (var i = 0; i < e.length; i++) { var g = true; for (var j in c) { switch (j) { case "className": f = e[i].getAttribute("class") ? "class" : "className"; break; default: f = j; break } if (e[i].getAttribute(f) != c[j]) g = false; if (g) d.push(e[i]) } } return d } }, $: function (a, b, c) { return this.dom.$(a, b, c) }, getElementsByClassName: function (a, b) { a = a || document; return a.getElementsByClassName ? a.getElementsByClassName(b) : document.getElementsByClassName(b, a) }, each: function (a, b) { if (a) { for (var i = 0; i < a.length; i++) b(i, a[i]) } }, style: function (a, b) { var e = this.dom.$(a); if (!e) return null; if (b) { if (b instanceof Object) { for (var s in b) { var c = s.split("-"); for (var i = 1; i < c.length; i++) { c[i] = c[i].replace(c[i].charAt(0), c[i].charAt(0).toUpperCase()) } var d = c.join(''); e.style[d] = b[s] } } else if (b instanceof String) { e.style.cssText = b } } return e.currentStyle || document.defaultView.getComputedStyle(e, null) }, request: function (a, b) { var c = new RegExp('(\\?|&)' + a + '=([^&]+)&*').exec(b ? b : location.href); return c ? c[2] : '' }, supportHTML5: navigator.geolocation != null, template: function (a, b) { if (b instanceof Object) { var c = new RegExp(); for (var n in b) { c.compile('%' + n + '%|\{' + n + '\}', 'g'); a = a.replace(c, b[n]) } } return a }, screen: { width: function () { return Math.max(document.body.clientWidth, document.documentElement.clientWidth) }, height: function () { return Math.max(document.body.clientHeight, document.documentElement.clientHeight) }, offsetWidth: function () { return Math.max(document.body.offsetWidth, document.documentElement.offsetWidth) }, offsetHeight: function () { return Math.max(document.body.offsetHeight, document.documentElement.offsetHeight) } }, event: { add: function (a, c, d, b) { if (!a.attachEvent && !a.addEventListener) { alert('event error! parameter:' + ele + ',event:' + c); return } document.attachEvent ? a.attachEvent('on' + c, d) : a.addEventListener(c, d, b) }, remove: function (a, c, d, b) { document.detachEvent ? a.detachEvent('on' + c, d) : a.removeEventListener(c, d, b) }, src: function (a) { var e = a ? a : window.event; return e.target || e.srcElement }, stopBubble: function (a) { var e = a ? a : window.event; if (window.event) { e.cancelBubble = true } else { e.stopPropagation() } }, preventDefault: function (a) { if (window.event) { window.event.returnvalue = false; return false } else { a.preventDefault(); return false } } }, xhr: { max_request: 2, http_stack: null, proc_stack: [], init: function () { if (this.http_stack) return; this.http_stack = []; for (var i = 0; i < this.max_request; i++) { this.http_stack[i] = { state: 0, http: window.XMLHttpRequest ? new XMLHttpRequest() : (new ActiveXObject("MSXML2.XMLHTTP") || new ActiveXObject("MICROSOFT.XMLHTTP")) } } }, request: function (d, e) { this.init(); var f = { uri: d.uri || location.href, params: d.params, method: (d.method || "GET").toUpperCase(), async: d.async == false ? false : d.async || true, data: (d.data || 'text').toLowerCase(), random: d.random == false ? false : d.random || true, call: e }; if (d.params instanceof Object) { var g = 0; f.params = ''; for (var i in d.params) { if (g++ != 0) { f.params += '&' } f.params += i + '=' + encodeURIComponent(d.params[i]) } } if (f.call.start != null) f.call.start(); if (f.method != "POST" && f.random) { if (f.uri.indexOf('#') == -1) { if (f.uri.indexOf("?") == -1) f.uri += "?t=" + Math.random(); else f.uri += "&t=" + Math.random() } } var h = function (a, b, c) { a.state = 1; a.http.open(c.method, c.uri, c.async); a.http.onreadystatechange = function () { if (a.http.readyState == 4) { if (a.http.status == 200) { if (c.call.success) { a.state = 0; b.proc_stack.pop(); c.call.success(c.data == "text" ? a.http.responseText : (c.data == 'json' ? window.toJson(a.http.responseText) : a.http.responseXML)) } } else if (c.call.error) { a.state = 0; b.proc_stack.pop(); c.call.error(a.http.responseText) } } }; if (c.method == "POST") a.http.setRequestHeader("Content-Type", "application/x-www-form-urlencoded"); a.http.send(c.params) }; (function (a, b) { var t = setInterval(function () { if (a.proc_stack.length < a.max_request) { a.proc_stack.push(0); for (var i = 0; i < a.max_request; i++) { if (a.http_stack[i].state == 0) { h(a.http_stack[i], a, b); break } } clearInterval(t) } }, 20) }(this, f)) }, get: function (a, b, c) { this.request(a instanceof Object ? a : { uri: a }, { success: function (x) { if (b) b(x) }, error: function (x) { if (c) c(x) } }) }, post: function (a, b, c, d) { this.request({ uri: a, method: 'POST', params: b }, { success: function (x) { if (c) c(x) }, error: function (x) { if (d) d(x) } }) }, jsonPost: function (b, c, d, e) { b += ((b || location.href).indexOf('?') == -1 ? '?' : '&') + 'json=1'; this.request({ uri: b, params: c, method: 'POST', data: 'json' }, { success: function (a) { if (d) d(a) }, error: function (a) { if (e) e(a) } }) } }, cookie: { write: function (a, b, c) { var d = ""; if (c) { d = new Date((new Date()).getTime() + c); d = "; expires=" + d.toGMTString() } document.cookie = a + "=" + escape(b) + d }, remove: function (a) { var b = new Date(); b.setTime(b.getTime() - 1); document.cookie = a + "=; expires=" + b.toGMTString() }, read: function (a) { var b = ""; var c = a + "="; if (document.cookie.length > 0) { var d = document.cookie.indexOf(c); if (d != -1) { d += c.length; var e = document.cookie.indexOf(";", d); if (e == -1) e = document.cookie.length; b = unescape(document.cookie.substring(d, e)) } } return b } }, json: { prefix: 'field', _objReg: /(.+)\[([^\]]+)\]/, _dtReg: /^(\d{4}((\/|-)\d{2}){2})T(\d{2}(:\d{2}){2})((\.\d+)*)$/i, _each: function (a, b) { for (var i = 0; i < a.length; i++) { if (b) b(i, a[i]) } }, _getFields: function (b) { var c = this.prefix; var d = {}; var f; var g, subProName, proValue; if (!b.nodeName) b = document.getElementById(b); var h = this._objReg; this._each(b.getElementsByTagName('*'), function (i, e) { if (e.nodeName != '#text' && e.nodeName != '#comment') { g = e.getAttribute(c); if (g) { if (h.test(g)) { var a = h.exec(g); g = a[1]; subProName = a[2]; if (d[g] == null) { d[g] = {} } d[g][subProName] = e } else { d[g] = e } if (!e.name) e.setAttribute('name', c + '_' + g) } } }); return d }, _bindField: function (a, b) { if (this._dtReg.test(b)) { var c = this._dtReg.exec(b); if (c[4] == '00:00:00') { b = b.replace(this._dtReg, '$1') } else { b = b.replace(this._dtReg, '$1 $4') } } switch (a.nodeName) { case 'TEXTAREA': case 'INPUT': switch (a.type) { default: a.value = b; break; case "radio": var d = document.getElementsByName(a.name); for (var i = 0; i < d.length; i++) { if (d[i].value == b) { d[i].setAttribute('checked', 'checked'); break } } break; case 'checkbox': var e = false; if ((b == true && b.toString() != '1') || b == a.value) { e = true } else if (b.length) { for (var i in b) { if (b[i] == a.value) { e = true; break } } } if (e) { a.setAttribute('checked', 'checked') } else { a.removeAttribute('checked') } break } break; case 'IMG': a.src = b; break; case 'SELECT': a.value = b; break; default: a.innerHTML = b; break } }, _getFieldVal: function (a) { var b = ''; switch (a.nodeName) { case 'TEXTAREA': case 'INPUT': switch (a.type) { default: b = a.value; break; case 'radio': var c = document.getElementsByName(a.name); for (var i = 0; i < c.length; i++) { if (c[i].checked) { b = c[i].value; break } } break; case 'checkbox': b = a.checked ? a.value : ''; break } break; case 'IMG': b = a.src; break; case 'SELECT': b = a.selectedIndex == -1 ? '' : a.options[a.selectedIndex].value; break; default: b = a.innerHTML; break } return b }, bind: function (a, b, c) { var d; var e; var f; d = this._getFields(a); for (var g in d) { e = d[g]; if (c && c instanceof Function) { f = c(g, b[g]) } else { f = b[g] } if (f != null) { if (f instanceof Object) { if (f.length) { for (var i in e) { this._bindField(e[i], f) } } else { for (var i in f) { if (e[i]) { this._bindField(e[i], f[i]) } } } continue } this._bindField(e, f) } } }, __convert: function (a, b, c) { var d; var e; var f; var g = {}; var h = ''; d = this._getFields(a); for (var k in d) { e = d[k]; if (e.nodeName) { f = this._getFieldVal(e); if (c && c instanceof Function) { f = c(k, f) } g[k] = f; h += k + '=' + f + '&' } else { g[k] = {}; var j = 0; var l = false; for (var i in e) { if (j++ == 0 && /^\d+$/.test(i)) { g[k] = []; l = true } if (e[i]) { f = this._getFieldVal(e[i]); if (c && c instanceof Function) { f = c(k, f) } if (f && f != '') { if (l) { g[k].push(f) } else { g[k][i] = f } } h += k + '[' + i + ']=' + f + '&' } } } } return b == "object" ? g : h.replace(/&$/g, '') }, toObject: function (a) { return this.__convert(a, 'object') }, toQueryString: function (a) { return this.__convert(a, 'string') }, toString: function (a) { return this.__convert(a, 'string').replace(/&/g, ';').replace(/=/g, ':') }, string: function (o) { var a = this; var b = []; var c = function (s) { if (typeof s == 'object' && s != null) a.string(s); return /^(string|number)$/.test(typeof s) ? "'" + s + "'" : s }; for (var i in o) { val = c(o[i]); if (val.pop) { b.push("'" + i + "':[\'" + escape(val.join('\',\'')) + '\']') } else { b.push("'" + i + "':" + escape(val)) } } return '{' + escape(b.join(',')) + '}' } }, getPosition: function (e) { return (e.nodeName ? e : this.$(e)).getBoundingClientRect() }, loadHTML: function (a, b) { var c = /<body[^>]*>([\s\S]+)<\/body>/im; var d = /<script((.|\n)*?)>([\s\S]*?)<\/script>/gim; var f = b.match(c); if (f == null) { f = ['', b] } if (!a.nodeName) a = this.dom.$(a); if (a) { try { a.innerHTML = f[1].replace(d, '').replace(/<style([^>]+)>/ig, '<span style="display:none" class=\"forie\">_</span><style$1>'); this.each(a.getElementsByClassName ? a.getElementsByClassName('forie') : document.getElementsByClassName('forie', a), function (i, e) { a.removeChild(e) }); if (window.navigator.userAgent.indexOf('Chrome') != -1) { this.each(a.getElementsByTagName('STYLE'), function (i, e) { a.removeChild(e); document.getElementsByTagName('HEAD')[0].appendChild(e) }) } } catch (ex) { if (window.console) { console.log(ex.message) } } } var g = /^[\n\s]+$/g; var h = /type=["']*text\/javascript["']*/i; var j; d.lastIndex = 0; while ((j = d.exec(b)) != null) { if (j[1].indexOf(' type=') == -1 || h.test(j[1])) { if (!g.test(j[3])) { this.eval(j[3]) } } } }, load: function (c, d, e, f) { (function (b) { b.xhr.get(d, function (a) { b.loadHTML(c, a); if (e) { e(a) } }, f) }(this)) }, ld: function (c, d) { (function (j, b) { j.xhr.get({ uri: b + c + '.js', async: false, random: false }, function (a) { j.eval(a) }) }(this, d || this.__WORKPATH__)) }, toJson: function (a) { return window.toJson(a) }, eval: function (a) { if (!a) return a; if (window.execScript) { window.execScript(a) } else { var b = document.createElement('SCRIPT'); b.setAttribute('type', 'text/javascript'); b.text = a; document.head.appendChild(b); document.head.removeChild(b) } return a } }; js.plugin = js.extend; js.prototype.ie6 = function () { return /MSIE\s*6\.0/.test(window.navigator.userAgent) }; js.prototype.path = function () { var d = document.domain, uri = location.href; d = uri.substring(uri.indexOf(d) + d.length); return d.substring(d.indexOf("/")) }; js.prototype.val = function (a, b) { if (!b) return document.getElementById(a).value; else document.getElementById(a).value = b }; js.prototype.lazyRun = function (a, b) { if (a) { setTimeout(a, b || 120) } }; js.prototype.hover = function (e, a, b) { if (!e.nodeName) e = this.$(e); var c = this.ie6(); this.event.add(e, 'mouseover', (function (t) { return function () { if (c) t.className += ' hover'; if (a) a(t) } })(e)); this.event.add(e, 'mouseout', (function (t) { return function () { if (c) t.className = t.className.replace(' hover', ''); if (b) b(t) } })(e)) }; js.prototype.ldScript = function (a, b, c) { var d = null; var e = document.documentElement.getElementsByTagName("HEAD"); if (e.length != 0) d = e[0]; else d = document.body; var f = d.getElementsByTagName('SCRIPT'); var g = false; for (var i = 0; i < f.length; i++) { if (f[i].getAttribute('src') && f[i].getAttribute('src').toLowerCase() == a.toLowerCase()) { g = true } } if (!g) { var h = document.createElement('SCRIPT'); if (b) h.onreadystatechange = h.onload = b; if (c) h.onerror = c; h.setAttribute('type', 'text/javascript'); h.setAttribute('src', a); d.appendChild(h) } }; js.prototype._width = function (e, a) { e = this.$(e); var s = this.style(e); if (s["display"] != 'none') { return a ? e.clientWidth : e.offsetWidth } var b = {}; for (var i in s) { b[i] = s[i] } this.style(e, { position: 'absolute', visibility: 'hidden', display: 'block' }); var w = a ? e.clientWidth : e.offsetWidth; this.style(e, b); this.style(e, { display: 'none' }); return w }; js.prototype._height = function (e, a) { e = this.$(e); var s = this.style(e); if (s["display"] != 'none') { return a ? e.clientHeight : e.offsetHeight } var b = {}; for (var i in s) { b[i] = s[i] } this.style(e, { position: 'absolute', visibility: 'hidden', display: 'block' }); var h = a ? e.clientHeight : e.offsetHeight; b.display = 'none'; this.style(e, b); this.style(e, { display: 'none' }); return h }; js.prototype.width = function (e) { return this._width(e) }; js.prototype.height = function (e) { return this._height(e) }; js.prototype.clientWidth = function (e) { return this._width(e, true) }; js.prototype.clientHeight = function (e) { return this._height(e, true) }; window.$JS = new js(); window.$JS.__init__(); window.J = $JS;


$JS.event.add(window, 'load', function () { $JS.each(document.getElementsByClassName('ui-exchange'), function (i, e) { var v = null; var d = null; var f = null; var g = 'exchange'; switch (e.nodeName) { case 'IMG': f = 'src'; break; default: f = 'innerHTML'; break } if (f == null) return; v = e[f]; d = e.getAttribute(g); if (d) { $JS.event.add(e, 'mouseover', (function (a, b, c) { return function () { a[b] = c } })(e, f, d)); $JS.event.add(e, 'mouseout', (function (a, b, c) { return function () { a[b] = c } })(e, f, v)) } }) });

window.cms = window.$JS;

//设置工作路径
cms.__WORKPATH__ = '/framework/assets/js/';

/****************  页面处理事件 **************/
var _scripts = document.getElementsByTagName('SCRIPT');
var _sloc = _scripts[_scripts.length - 1].src;                                  //Script Location
var _hp = {                                                                //Script Handle Params
    loadUI: cms.request('ui', _sloc) == '1',                                     //load ui lib
    hoverNavi: cms.request('hover', _sloc).indexOf('navi') != -1,            //Hover Navigator
    hoverCList: cms.request('hover', _sloc).indexOf('clist') != -1,           //Hover Category List
    hoverAList: cms.request('hover', _sloc).indexOf('alist') != -1,           //Hover Archive List
    plugins: cms.request('ld', _sloc)
};

var plugins = null;
if (_hp.loadUI) {
    plugins = new Array('ui', 'scrollbar', 'scroller', 'form');
} else if (_hp.plugins) {
    plugins = _hp.plugins.split(',');
}
if (plugins) {
    for (var i = 0; i < plugins.length; i++) {
        cms.ld(plugins[i]);
    }
}

/************** 自动设置导航 *********************/
var _auto_checked_navigator = false;
var _auto_navigator_ele;
var _auto_navigator_timer;
if (_hp.hoverNavi) {
    var navi = null;
    _auto_navigator_timer = setInterval(function () {
        navi = document.getElementsByClassName('page-navigator');
        if (navi && navi.length != 0) {
            navi = navi[0];

            var loc = window.location.pathname;
            var lis = [];

            var lis2 = navi.getElementsByTagName('UL')[0].childNodes;
            $JS.each(lis2, function (i, e) {
                if (e.nodeName == "LI") {
                    lis.push(e);
                }
            });

            if (lis.length > 0) {
                var link;
                var isHovered = false;

                var setHover = function (_loc) {
                    for (var i = 0; i < lis.length; i++) {

                        link = lis[i].getElementsByTagName('A')[0];
                        if (link.href.indexOf(_loc) != -1) {
                            lis[i].className += lis[i].className.indexOf('current') == -1 ? ' current' : '';
                            isHovered = true;

                            break;
                        }
                    }
                };

                //全局匹配
                setHover(loc);

                if (!isHovered) {
                    var _loc = loc;
                    //模糊匹配
                    var i = 0;
                    while (!isHovered) {
                        var splitIndex = _loc.lastIndexOf('/');
                        if (splitIndex == _loc.length - 1) {
                            splitIndex = _loc.substring(0, _loc.length - 1).lastIndexOf('/');
                        }
                        _loc = _loc.substring(0, splitIndex + 1);
                        setHover(_loc);
                        if (++i > lis.length) {
                            isHovered = true;
                        }
                    }
                }
                setIE6Drop(lis);

            }
            clearInterval(_auto_navigator_timer);
        }
    }, 100);
}

function setIE6Drop(lis) {
    /****************** 设置二级菜单 *******************/
    //针对IE6不支持hover属性
    if (window.ActiveXObject) {
        var agent = window.navigator.userAgent;
        if (/MSIE\s*6\.0/.test(agent)) {

            for (var i = 0; i < lis.length; i++) {
                $JS.event.add(lis[i], 'mouseover', (function (t) {
                    return function () {
                        t.className += ' drop';
                    };
                })(lis[i]));

                $JS.event.add(lis[i], 'mouseout', (function (t) {
                    return function () {
                        t.className = t.className.replace(' drop', '');
                    };
                })(lis[i]));

            }
        }
    }
}


cms.event.add(window, 'load', function () {
    if (_hp.hoverNavi && _auto_navigator_ele) {
        clearInterval(_auto_navigator_timer);
    }

    var loc = window.location.pathname;


    /****************** 设置分类菜单 *******************/

    //根据className设置Hover状态
    var setHoverByClassName = function (e) {
        var lis = e.childNodes;
        var link;
        var isHovered = false;

        var setHover = function (_loc) {
            for (var i = 0; i < lis.length; i++) {
                if (lis[i].nodeName[0] == '#') continue;
                link = lis[i].getElementsByTagName('A')[0];
                if (link.href.indexOf(_loc) != -1) {
                    lis[i].className += lis[i].className.indexOf('current') == -1 ? ' current' : '';
                    isHovered = true;
                    break;
                }
            }
        };

        //全局匹配
        setHover(loc);

        //模糊匹配
         if (!isHovered) {
             var splitIndex = loc.lastIndexOf('/');
             if (splitIndex == loc.length - 1) {
                 splitIndex = loc.substring(0, loc.length - 1).lastIndexOf('/');
             }
             setHover(loc.substring(0, splitIndex + 1));
         }
    };

    // ***********************************
    // 设置CList选中效果 (2012-11-03) **
    // ***********************************
    var _e_clist = document.getElementsByClassName('clist');
    if (_hp.hoverCList && _e_clist.length != 0) {
         setHoverByClassName(_e_clist[0]);
    }

    // ***********************************
    // 设置AList选中效果 (2012-11-03) **
    // ***********************************
    _e_clist = document.getElementsByClassName('alist');
    if (_hp.hoverAList && _e_clist.length != 0) {
        setHoverByClassName(_e_clist[0]);
    }
});

/***********************  设置自动时间  ***********************/
/*
var ele_dts = document.getElementsByClassName('autotime');
var weeks = new Array('日', '一', '二', '三', '四', '五', '六');
var cmath = function (v) {
    if (v < 10) return '0' + v;
    return v;
};
var setDate = function () {
    var dt = new Date();
    var str = cmath(dt.getFullYear()) + '年' + cmath(dt.getMonth() + 1) + '月' + cmath(dt.getDate()) + '日&nbsp;/&nbsp;周' +
          weeks[dt.getDay()] + '&nbsp;' + cmath(dt.getHours()) + ':' + cmath(dt.getMinutes()) + ':' + cmath(dt.getSeconds());

    for (var i = 0; i < ele_dts.length; i++) {
        ele_dts[i].innerHTML = str;
    }
};
if (ele_dts.length != 0) {
    setDate();
    setInterval(setDate, 1000);
}
*/

