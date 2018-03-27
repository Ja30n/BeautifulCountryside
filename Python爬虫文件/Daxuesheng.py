#coding=gbk
import requests
import pyodbc
from bs4 import BeautifulSoup
url='http://zhannei.baidu.com/cse/search?q=%E5%AD%94%E5%9D%B5%E6%9D%91&p=0&s=10871809385446352931&entry=1'
url2='http://zhannei.baidu.com/cse/search?q=%E5%AD%94%E5%9D%B5%E6%9D%91&p=1&s=10871809385446352931&entry=1'
def fun(testurl):
    headers = {'user-agent': 'my-app/0.0.1'}
    res=requests.get(testurl,headers=headers)
    res.encoding='utf-8'
    soup=BeautifulSoup(res.text,'html.parser')

    cnxn =pyodbc.connect('DRIVER={SQL Server};SERVER=localhost;DATABASE=GDKQ_DB;UID=sa;PWD=123456')
    cursor = cnxn.cursor()


    for i in soup.select('#results')[0].find_all("a"):
        res2 = requests.get(i.get("href"))
        print('文章链接：'+i.get("href"))




        res2.encoding = 'utf-8'
        soup2 = BeautifulSoup(res2.text, 'html.parser')
        # print(soup.select('.title'))
        Title=soup2.select('.title')[0].h1.text.strip()
        ExctraMess=soup2.select('.title')[0].span.text.strip()
        


        TurlBody=''
        for j in soup2.select('.content')[0].find_all("p"):
            TurlBody=TurlBody+j.text.strip()
        #print(TurlBody)
        #tempstring='insert into dbo.Py_Spider(Title, Body) values (%s,%s)'
        #tempstring.format(Title,TurlBody)
        #
        var1="01"#CategoryID
        var2="16"#AuthorID
        var3="大学生新闻网"#lab
        var4="最新文章"#CaName
        sadf="insert into dbo.Article(Title, Body,AuthorID,CategoryID,lab,CaName,AuthorName) values ("+"'"+Title+"'"+","+"'"+TurlBody+"'"+","+"'"+var2+"'"+","+"'"+var1+"'"+","+"'"+var3+"'"+","+"'"+var4+"'"+","+"'"+var3+"'"+")"
        #print(sadf)
        cursor.execute(sadf)
        cnxn.commit()
        #cursor.execute("insert into dbo.Py_Spider(Title, Body,Creattime,IsDelect,Enable,remark) values (%s,%s,%s,%s,%s,%s)",'Title','TurlBody','null',True,True,'null')
        #cnxn.commit()

        #cursor.execute("insert into dbo.Py_Spider(Title, Body) values ('Title', 'TurlBody')")
        #cnxn.commit()
        #print(tempstring)
    #print(i.get("href"),i.text)
fun(url)
fun(url2)
