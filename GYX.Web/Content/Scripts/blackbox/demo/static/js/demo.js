/**
 * Author: Vincent Ting
 * Date: 13-3-10
 * Time: 下午8:57
 */
(function () {

    var box = new BlackBox(),

        demos = {

            alert_preview: function () {
                box.alert("这里是一个 alert 的 demo", function () {
                    alert("窗口即将自己关闭")
                }, {
                    icon: 'info',
                    title: '这里是标题',
                    value: '确定'
                })
            },

            confirm_preview: function () {
                box.confirm("这里是一个 confirm 的 demo", function (data) {
                    if (data) {
                        box.alert("你点击了确定")
                    } else {
                        box.alert("你点击了取消")
                    }
                }, {
                    title: '这里是标题',
                    value: '确定'
                })
            },

            prompt_preview: function () {
                box.prompt("请输入一个大于 10 的数字", function (data) {
                    if (data) {
                        box.alert("你刚刚输入了" + data)
                    } else {
                        box.alert("你放弃了输入")
                    }
                }, {
                    title: '这里是标题',
                    value: '确定',
                    verify: function (data) {
                        return data > 10;
                    }
                })
            },

            load_preview: function () {
                box.load("index", function () {
                    console.log("第一个index载入内容载入完毕")
                });
                box.load("index", function () {
                    console.log("第二个index载入内容载入完毕")
                });
                box.load("main", function () {
                    console.log("main载入内容载入完毕")
                });
                setTimeout(function () {
                    box.ready("main");
                }, 1000);
                setTimeout(function () {
                    box.ready("index")
                }, 2000);
            },

            popup_preview: function () {
                box.popup('内容',
                    function (content) {
                        //content.find("#shake_demo").click(function () {
                        //    box.boxShake();
                        //});
                        //content.find("#close_demo").click(function () {
                        //    box.boxClose();
                        //});
                    },
                    {
                        hasOk: true,//确定按钮
                        hasCancel: false,//取消按钮
                        width: 400,//窗口宽度
                        height: 200,//窗口高度
                        left: 100,//窗口左侧距离
                        top: 20//窗口上方距离
                    }

                    );
            },

            list_preview: function () {
                box.alert("Hello word");
                box.load("index", function () {
                    console.log("index 已经完成了");
                    box.alert("之前的 index 已经完成了");
                });
                setTimeout(function () {
                    box.ready("index");
                }, 2000);
                box.confirm("Bye word", {
                    title: "再见世界",
                    okText: '再见',
                    cancelText: '回来'
                });
            }

        };


    $(document).ready(function () {

        try {
            hljs.initHighlightingOnLoad();

        } catch (e) {

        }

        //box.alert("Hello World - BlackBox ~", {
        //    title: '开始探索',
        //    value: '出发!'
        //});

        $("button[class=preview]").each(function () {
            var id = $(this).attr("id");
            if (demos[id]) {
                $(this).click(demos[id]);
            }
        })

    })


}).call(window);