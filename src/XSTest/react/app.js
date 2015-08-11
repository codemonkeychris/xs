/// <reference path='../../xsrt2/xsrt.d.ts' />
var Notebook;
(function (Notebook) {
    function pad(pad, str, padLeft) {
        if (typeof str === 'undefined')
            return pad;
        if (padLeft) {
            return (pad + str).slice(-pad.length);
        }
        else {
            return (str + pad).substring(0, pad.length);
        }
    }
    function repeat(pattern, count) {
        if (count < 1)
            return '';
        var result = '';
        while (count > 1) {
            if (count & 1)
                result += pattern;
            count >>= 1, pattern += pattern;
        }
        return result + pattern;
    }
    function mmax(v1, v2) {
        if (v1 === null || v1 === undefined)
            return v2;
        if (v1 < v2)
            return v2;
        return v1;
    }
    function computeColumns(items) {
        var columns = [];
        var proto = items.reduce(function (previousProto, currentItem) {
            Object.keys(currentItem).forEach(function (currentKey) { return previousProto[currentKey] = mmax(previousProto[currentKey], formatString(currentItem[currentKey]).length); });
            return previousProto;
        }, {});
        Object.keys(proto).forEach(function (key) { return columns.push({ name: key, width: Math.min(proto[key], 50) }); });
        return columns;
    }
    function createHeader(columns) {
        return columns.map(function (column) { return formatString(column.name, column); });
    }
    function formatString(value, columnInfo) {
        var baseString = "" + value;
        if (columnInfo) {
            return pad(repeat(' ', columnInfo.width), baseString, true);
        }
        else {
            return baseString;
        }
    }
    function createRow(data, columns) {
        return columns.map(function (column) { return formatString(data[column.name], column); });
    }
    function joinCells(cells) {
        return cells.join("|");
    }
    function joinRows(rows) {
        return rows.join("\n");
    }
    function startsWith(str, search) {
        return str.slice(0, search.length) == search;
    }
    ;
    function scopedEval(js, context) {
        var result;
        // with (context) {
        result = eval(js);
        //}
        return result;
    }
    function textTable(items) {
        var columns = computeColumns(items);
        var header = joinCells(createHeader(columns));
        var rows = [];
        rows.push(header);
        rows = rows.concat(items.map(function (item) { return joinCells(createRow(item, columns)); }));
        return joinRows(rows);
    }
    Notebook.textTable = textTable;
    function process(notebook, context) {
        var evals = [];
        var renumberInOut = true; // change if you want to honor the document
        var inCount = 0;
        var lines = notebook.split('\n');
        var currentLine = 0;
        var rows = [];
        function calcBody(restOfLine) {
            if (!restOfLine || restOfLine.trim().length === 0) {
                var s = "";
                if (peekLine().trim() === "```") {
                    takeLine(); // ``` 
                    var l = "";
                    while ((l = peekLine()) && l.trim() !== "```") {
                        s += takeLine().trim() + "\n";
                    }
                    takeLine(); // ```
                }
                return { value: s };
            }
            else if (restOfLine[0] === "`" && restOfLine[restOfLine.length - 1] === "`") {
                return { value: restOfLine.substring(1, restOfLine.length - 1), codeMarker: true };
            }
            return { value: restOfLine };
        }
        function pushResult(preamble, body, codeMarker, noteMarker) {
            var prepre = noteMarker ? "> " : "";
            if (body.indexOf('\n') != -1) {
                pushOut(prepre + preamble);
                pushOut("```");
                pushOut(body.replace(/\s*$/gm, '')); //trimRight
                pushOut("```");
            }
            else if (codeMarker) {
                pushOut(prepre + preamble + "`" + body + "`");
            }
            else {
                pushOut(prepre + preamble + body);
            }
        }
        function processLine(line) {
            var inMatch = /^(>\s+)?in\[([0123456789]+)\]\s*=\s*((`(.*)`)|([^`].*)|())/;
            var outMatch = /^(>\s+)?out\[([0123456789]+)\]\s*=\s*((`(.*)`)|([^`].*)|())/;
            var inResult = inMatch.exec(line);
            var outResult = outMatch.exec(line);
            if (inResult) {
                // debugger;
                var body = calcBody(inResult[3]);
                body.noteMarker = !!inResult[1];
                var index = 0 || inResult[2];
                if (renumberInOut) {
                    index = inCount;
                }
                inCount++;
                evals[index] = scopedEval(body.value, context);
                pushResult("in[" + index + "]=", body.value, body.codeMarker, body.noteMarker);
            }
            else if (outResult) {
                var body = calcBody(outResult[3]); // need to consume old body
                body.noteMarker = !!outResult[1];
                var index = 0 || outResult[2];
                if (renumberInOut) {
                    index = inCount - 1; // take last in
                }
                var result = formatString(evals[index]);
                pushResult("out[" + index + "]=", result, body.codeMarker, body.noteMarker);
            }
            else {
                pushOut(line);
            }
        }
        function pushOut(output) { rows.push(output); }
        function peekLine() { return !end() ? lines[currentLine] : null; }
        function takeLine() { return lines[currentLine++]; }
        function end() { return currentLine >= lines.length; }
        while (!end()) {
            processLine(takeLine());
        }
        return joinRows(rows);
    }
    Notebook.process = process;
})(Notebook || (Notebook = {}));
var App;
(function (App) {
    function MultiLineTextBox() {
        return React.createElement(Xaml.TextBox, {"scrollViewer$horizontalScrollBarVisibility": 'Auto', "scrollViewer$verticalScrollBarVisibility": 'Auto', "acceptsReturn": true, "textWrapping": 'NoWrap', "horizontalAlignment": 'Stretch', "verticalAlignment": 'Stretch'});
    }
    var data = [{ x: 12, y: 20 }, { x: 2000, y: 20 }, { x: 30, y: 20 }, { x: 30, y: 20 }];
    var input = "" +
        "# JSNotebook... a simple way to write up JS code... \n" +
        "\n" +
        "### Basic expressions\n" +
        "\n" +
        "in[0]=40+40\n" +
        "\n" +
        "You can optionally surround the expression with Markdown inline code \n" +
        "markings and prefix with the note (>) marker if you like. \n" +
        "\n" +
        "> in[1]=`40+40`\n" +
        "\n" +
        "The result will be put after any out statement:\n" +
        "\n" +
        "> out[1]=80\n" +
        "\n" +
        "### Multiline expressions are done using Markdown syntax for code:\n" +
        "> in[2]=\n" +
        "```\n" +
        "40 +\n" +
        "40\n" +
        "```\n" +
        "> out[2]=80\n" +
        "\n" +
        "### Variables can be defiend\n" +
        "> in[3]=x=10\n" +
        "\n" +
        "> in[4]=x*20\n" +
        "\n" +
        "> out[4]=200\n" +
        "\n" +
        "> in[5]=data = [ {x:12, y:20 }, {x:2000, y:20 }, {x:30, y:20 }, {x:30, y:20 } ]\n" +
        "\n" +
        "> out[5]=[object Object],[object Object],[object Object],[object Object]\n" +
        "\n" +
        "### There are several built in functions for formatting\n" +
        "> in[6]=Notebook.textTable(data)\n" +
        "\n" +
        "> out[6]=\n" +
        "```\n" +
        "   x| y\n" +
        "  12|20\n" +
        "2000|20\n" +
        "  30|20\n" +
        "  30|20\n" +
        "```\n";
    function keyDown(sender, e) {
        if (e.key === 116) {
            host.setState({ input: sender.text });
        }
    }
    function setInitialState() {
        host.setState({ input: input });
    }
    App.setInitialState = setInitialState;
    function render() {
        return (React.createElement(Xaml.Grid, {"rows": ['*', 'auto']}, React.createElement(Xaml.TextBlock, {"grid$row": 1}, "Press F5 to refresh"), React.createElement(MultiLineTextBox, {"grid$row": 0, "onKeyDown": keyDown, "fontFamily": 'Consolas', "margin": '10,10,10,10', "text": Notebook.process(host.getState().input, { data: data, textTable: Notebook.textTable })})));
    }
    App.render = render;
})(App || (App = {}));
;
