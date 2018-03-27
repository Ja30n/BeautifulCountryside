var billType1={payout:1,transferOut:2,borrow:3,borrowBack:4,income:5,lend:6,lendBack:7,transferIn:8,daifu:12};
var billManager1={
	addType:billType1.payout,realType:billType1.payout,moreIsShow:false,typePos:new Array(-1,0,2,3,3,1,3,3),money2IsShow:false,showMore:function(){if(billManager1.moreIsShow==true){this.closeMore()
	}else{
		$("#type-more").addClass("btnshow");$("#type-more-box").show();billManager1.moreIsShow=true}
		},closeMore:function(){
			if(billManager1.moreIsShow==true)
			{$("#type-more-box").hide();$("#type-more").removeClass("btnshow");billManager1.moreIsShow=false}
			},moreButtonClick:function(type)
			{
				$("#tz-3,#tz-4,#tz-6,#tz-7,#tz-12,#tz-l-3,#tz-l-4,#tz-l-6,#tz-l-7,#tz-l-12").remove();
				$("#tzls_box-ul").append('<li class="tz-l" id="tz-l-'+type+'"></li><li class="tz-n" id="tz-'+type+'"><a onclick="javascript:billManager1.changeType('+type+');"></a></li>');addMouseStyle($("#tz-"+type+" a"),"hover","active");
				$("#tz-"+type+" a").click();
				},changeType:function(typeId){
					var pos=$("#tzls_box-ul .tz-n").index($("#tz-"+typeId));$("#tzls_box li.tz-l").removeClass("tz-l-no");
				$("#tzls_box li.tz-l").eq(pos).addClass("tz-l-no");if(pos>0)
				{
					$("#tzls_box li.tz-l").eq(pos-1).addClass("tz-l-no")
				}$("#tzls_box a").removeClass("select");$("#tz-"+typeId+" a").addClass("select");
					
						$("#tz-m .tz-ul-1").hide();
						$("#tzul-"+typeId).show();
								}
													};

