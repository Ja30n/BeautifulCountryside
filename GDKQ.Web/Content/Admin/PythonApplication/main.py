#coding='utf-8'
import requests
import pyodbc
import re
from bs4 import BeautifulSoup

url='http://www.zjtz.gov.cn/col/col14/index.html'
headers = {'user-agent': 'my-app/0.0.1'}
res=requests.get(url,timeout=500,headers=headers)
res.encoding='utf-8'
temp=res.text.replace('&lt;','<').replace('&gt;','>').replace('&amp;','&').replace('![CDATA[','').replace(']]','').replace('&quot;','"').replace('$apos;',"'")
soup=BeautifulSoup(temp,'html.parser')
cnxn =pyodbc.connect('DRIVER={SQL Server};SERVER=localhost;DATABASE=GDKQ_DB;UID=sa;PWD=123456')
cursor = cnxn.cursor()


urls='http://www.zjtz.gov.cn'
for i in soup.select('#6184')[0].find_all("a")[1:]:
    realurl=urls+i.get("href")
    print(realurl)
    res2 = requests.get(realurl)
    res2.encoding = 'utf-8'
    soup2 = BeautifulSoup(res2.text, 'html.parser')
   # print('标题：' + soup2.select('#title')[0].text.strip() + '\n' + '日期和来源：' + soup2.find(style="font-size:12px; color:#999; line-height:24px; padding-top:10px;").text.strip() + '\n' +'正文内容：'+ soup2.select('#zoom')[0].text)
   # print("---------------------------------")

	
    #print(TurlBody)
    #tempstring='insert into dbo.Py_Spider(Title, Body) values (%s,%s)'
    #tempstring.format(Title,TurlBody)
    #temp=re.findall('%S*',)
    #print(temp)
    #print(soup2.select('#zoom')[0])
    #sfjgkl=str(soup2.select('#zoom')[0].content)
    sadf="insert into dbo.Py_Spider(Title, Body) values ("+"'"+soup2.select('#title')[0].text.strip()+"'"+","+"'"+soup2.select('#zoom')[0].text.strip()+"'"+")"
    #print(sadf)
    cursor.execute(sadf)
    cnxn.commit()
