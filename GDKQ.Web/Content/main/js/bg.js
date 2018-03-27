var billType={payout:1,transferOut:2,borrow:3,borrowBack:4,income:5,lend:6,lendBack:7,transferIn:8,daifu:12};
var billManager={
	addType:billType.payout,realType:billType.payout,moreIsShow:false,typePos:new Array(-1,0,2,3,3,1,3,3),money2IsShow:false,showMore:function(){if(billManager.moreIsShow==true){this.closeMore()
	}else{
		$("#type-more").addClass("btnshow");$("#type-more-box").show();billManager.moreIsShow=true}
		},closeMore:function(){
			if(billManager.moreIsShow==true)
			{$("#type-more-box").hide();$("#type-more").removeClass("btnshow");billManager.moreIsShow=false}
			},moreButtonClick:function(type)
			{
				$("#tm-3,#tm-4,#tm-6,#tm-7,#tm-12,#tm-l-3,#tm-l-4,#tm-l-6,#tm-l-7,#tm-l-12").remove();
				$("#type-menu-ul").append('<li class="tm-l" id="tm-l-'+type+'"></li><li class="tm-n" id="tm-'+type+'"><a onclick="javascript:billManager.changeType('+type+');"></a></li>');addMouseStyle($("#tm-"+type+" a"),"hover","active");
				$("#tm-"+type+" a").click();
				},changeType:function(typeId){
					var pos=$("#type-menu-ul .tm-n").index($("#tm-"+typeId));$("#type-menu li.tm-l").removeClass("tm-l-no");
				$("#type-menu li.tm-l").eq(pos).addClass("tm-l-no");if(pos>0)
				{
					$("#type-menu li.tm-l").eq(pos-1).addClass("tm-l-no")
				}$("#type-menu a").removeClass("select");$("#tm-"+typeId+" a").addClass("select");
					
						$("#tb-m .tb-ul-1").hide();
						$("#tbul-"+typeId).show();
								}
													};

