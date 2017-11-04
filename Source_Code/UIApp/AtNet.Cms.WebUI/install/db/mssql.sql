if exists (select * from sysobjects where id = OBJECT_ID('[cms_archive]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_archive]

CREATE TABLE [cms_archive] (
[aid] [int]  IDENTITY (1, 1)  NOT NULL,
[id] [nvarchar]  (48) NOT NULL,
[alias] [nvarchar]  (150) NULL,
[cid] [int]  NOT NULL,
[author] [nvarchar]  (150) NULL,
[title] [nvarchar]  (300) NULL,
[small_title] [nvarchar]  (300) NULL,
[source] [nvarchar]  (150) NULL,
[tags] [nvarchar]  (300) NULL,
[outline] [nvarchar]  (765) NULL,
[content] [ntext]  NULL,
[properties] [nvarchar]  (1500) NULL,
[viewcount] [int]  NULL DEFAULT (0),
[agree] [int]  NULL,
[disagree] [int]  NULL,
[createdate] [datetime]  NULL,
[lastmodifydate] [datetime]  NULL,
[flags] [nvarchar]  (300) NULL DEFAULT ('{st:''''0'''',sc:''''0'''',v:''''1'''',p:''''0''''}'),
[thumbnail] [nvarchar]  (450) NULL)

ALTER TABLE [cms_archive] WITH NOCHECK ADD  CONSTRAINT [PK_cms_archive] PRIMARY KEY  NONCLUSTERED ( [aid],[id],[alias],[cid] )
SET IDENTITY_INSERT [cms_archive] ON

INSERT [cms_archive] ([aid],[id],[alias],[cid],[author],[title],[content],[properties],[viewcount],[agree],[disagree],[createdate],[lastmodifydate],[flags]) VALUES ( 1,N'spcnet',N'welcome',2,N'admin',N'��ӭʹ��Special Cms .NET',N'<div style="text-align:center;font-size:30px"><h2>??�騨??????����Special Cms .NET!</h2></div>',N'{}',1,0,0,N'2013/1/1 1:01:01',N'2013/1/1 1:01:01',N'{st:''0'',sc:''0'',v:''1'',p:''0''}')

SET IDENTITY_INSERT [cms_archive] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_category]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_category]

CREATE TABLE [cms_category] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[siteid] [int]  NULL DEFAULT (1),
[moduleid] [int]  NOT NULL,
[tag] [nvarchar]  (300) NULL,
[name] [nvarchar]  (300) NULL,
[lft] [int]  NULL,
[rgt] [int]  NULL,
[pagetitle] [nvarchar]  (600) NULL,
[keywords] [nvarchar]  (600) NULL,
[description] [nvarchar]  (750) NULL,
[orderindex] [int]  NULL DEFAULT (0))

ALTER TABLE [cms_category] WITH NOCHECK ADD  CONSTRAINT [PK_cms_category] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_category] ON

INSERT [cms_category] ([id],[siteid],[moduleid],[tag],[name],[lft],[rgt],[orderindex]) VALUES ( 1,0,1,N'root',N'����Ŀ',1,4,0)
INSERT [cms_category] ([id],[siteid],[moduleid],[tag],[name],[lft],[rgt],[orderindex]) VALUES ( 2,1,1,N'cms',N'��ӭʹ��',2,3,0)

SET IDENTITY_INSERT [cms_category] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_comment]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_comment]

CREATE TABLE [cms_comment] (
[id] [int]  NOT NULL,
[archiveid] [nvarchar]  (48) NULL,
[memberid] [int]  NULL,
[ip] [nvarchar]  (60) NULL,
[content] [ntext]  NULL,
[recycle] [smallint]  NULL,
[createdate] [datetime]  NULL)

ALTER TABLE [cms_comment] WITH NOCHECK ADD  CONSTRAINT [PK_cms_comment] PRIMARY KEY  NONCLUSTERED ( [id] )
if exists (select * from sysobjects where id = OBJECT_ID('[cms_dataExtend]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_dataExtend]

CREATE TABLE [cms_dataExtend] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[name] [nvarchar]  (45) NULL,
[state] [int]  NULL DEFAULT (1))

ALTER TABLE [cms_dataExtend] WITH NOCHECK ADD  CONSTRAINT [PK_cms_dataExtend] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_dataExtend] ON


SET IDENTITY_INSERT [cms_dataExtend] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_dataExtendAttr]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_dataExtendAttr]

CREATE TABLE [cms_dataExtendAttr] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[extId] [int]  NOT NULL,
[attrName] [nvarchar]  (150) NOT NULL,
[attrType] [nvarchar]  (300) NOT NULL,
[attrVal] [nvarchar]  (60) NULL,
[regex] [nvarchar]  (300) NULL,
[attrMsg] [nvarchar]  (300) NULL,
[enabled] [int]  NULL DEFAULT (1))

ALTER TABLE [cms_dataExtendAttr] WITH NOCHECK ADD  CONSTRAINT [PK_cms_dataExtendAttr] PRIMARY KEY  NONCLUSTERED ( [id],[extId] )
SET IDENTITY_INSERT [cms_dataExtendAttr] ON


SET IDENTITY_INSERT [cms_dataExtendAttr] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_dataExtendField]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_dataExtendField]

CREATE TABLE [cms_dataExtendField] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[rid] [int]  NULL,
[extId] [int]  NULL,
[attrId] [int]  NULL,
[attrVal] [nvarchar]  (1500) NULL)

ALTER TABLE [cms_dataExtendField] WITH NOCHECK ADD  CONSTRAINT [PK_cms_dataExtendField] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_dataExtendField] ON


SET IDENTITY_INSERT [cms_dataExtendField] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_link]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_link]

CREATE TABLE [cms_link] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[pid] [int]  NULL,
[siteid] [int]  NULL DEFAULT (1),
[type] [int]  NOT NULL,
[text] [nvarchar]  (300) NOT NULL,
[uri] [nvarchar]  (765) NOT NULL,
[target] [nvarchar]  (150) NULL,
[imgurl] [nvarchar]  (300) NULL,
[bind] [nvarchar]  (60) NULL,
[index] [int]  NULL,
[visible] [smallint]  NOT NULL)

ALTER TABLE [cms_link] WITH NOCHECK ADD  CONSTRAINT [PK_cms_link] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_link] ON

INSERT [cms_link] ([id],[pid],[siteid],[type],[text],[uri],[target],[index],[visible]) VALUES ( 1,0,1,2,N'SPC.NET',N'http://www.ops.cc/cms/',N'_blank',2,1)
INSERT [cms_link] ([id],[pid],[siteid],[type],[text],[uri],[index],[visible]) VALUES ( 2,1,1,1,N'��ҳ',N'/',1,1)
INSERT [cms_link] ([id],[pid],[siteid],[type],[text],[uri],[index],[visible]) VALUES ( 3,0,1,1,N'��ӭʹ��',N'/cms/welcome.html',2,1)

SET IDENTITY_INSERT [cms_link] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_log]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_log]

CREATE TABLE [cms_log] (
[id] [nvarchar]  (90) NOT NULL,
[typeid] [int]  NULL,
[description] [nvarchar]  (765) NULL,
[content] [ntext]  NULL,
[helplink] [nvarchar]  (765) NULL,
[recorddate] [datetime]  NULL)

ALTER TABLE [cms_log] WITH NOCHECK ADD  CONSTRAINT [PK_cms_log] PRIMARY KEY  NONCLUSTERED ( [id] )
if exists (select * from sysobjects where id = OBJECT_ID('[cms_memberdetails]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_memberdetails]

CREATE TABLE [cms_memberdetails] (
[uid] [int]  NOT NULL,
[status] [nvarchar]  (30) NULL,
[regip] [nvarchar]  (45) NULL,
[regtime] [datetime]  NULL,
[lastlogintime] [datetime]  NULL,
[token] [nvarchar]  (300) NULL)

ALTER TABLE [cms_memberdetails] WITH NOCHECK ADD  CONSTRAINT [PK_cms_memberdetails] PRIMARY KEY  NONCLUSTERED ( [uid] )
if exists (select * from sysobjects where id = OBJECT_ID('[cms_member]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_member]

CREATE TABLE [cms_member] (
[id] [int]  NOT NULL,
[username] [nvarchar]  (60) NULL,
[password] [nvarchar]  (120) NULL,
[avatar] [nvarchar]  (765) NULL,
[sex] [nvarchar]  (21) NULL,
[nickname] [nvarchar]  (45) NULL,
[email] [nvarchar]  (150) NULL,
[telphone] [nvarchar]  (60) NULL,
[note] [nvarchar]  (765) NULL,
[usergroupid] [int]  NULL)

ALTER TABLE [cms_member] WITH NOCHECK ADD  CONSTRAINT [PK_cms_member] PRIMARY KEY  NONCLUSTERED ( [id] )
if exists (select * from sysobjects where id = OBJECT_ID('[cms_message]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_message]

CREATE TABLE [cms_message] (
[id] [int]  NOT NULL,
[senduid] [int]  NULL,
[receiveuid] [int]  NULL,
[subject] [nvarchar]  (150) NULL,
[content] [nvarchar]  (765) NULL,
[hasread] [smallint]  NULL,
[recycle] [smallint]  NULL,
[senddate] [datetime]  NULL)

ALTER TABLE [cms_message] WITH NOCHECK ADD  CONSTRAINT [PK_cms_message] PRIMARY KEY  NONCLUSTERED ( [id] )
if exists (select * from sysobjects where id = OBJECT_ID('[cms_modules]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_modules]

CREATE TABLE [cms_modules] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[siteid] [int]  NULL DEFAULT (1),
[name] [nvarchar]  (150) NOT NULL,
[issystem] [smallint]  NULL,
[isdelete] [smallint]  NULL,
[extid1] [int]  NULL DEFAULT (0),
[extid2] [int]  NULL DEFAULT (0),
[extid3] [int]  NULL DEFAULT (0),
[extid4] [int]  NULL DEFAULT (0))

ALTER TABLE [cms_modules] WITH NOCHECK ADD  CONSTRAINT [PK_cms_modules] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_modules] ON

INSERT [cms_modules] ([id],[siteid],[name],[issystem],[isdelete],[extid1],[extid2],[extid3],[extid4]) VALUES ( 1,0,N'�Զ���ҳ��',1,0,0,0,0,0)
INSERT [cms_modules] ([id],[siteid],[name],[issystem],[isdelete],[extid1],[extid2],[extid3],[extid4]) VALUES ( 2,0,N'�ĵ�',1,0,0,0,0,0)

SET IDENTITY_INSERT [cms_modules] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_operation]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_operation]

CREATE TABLE [cms_operation] (
[id] [bigint]  IDENTITY (1, 1)  NOT NULL,
[name] [nvarchar]  (765) NULL,
[path] [nvarchar]  (765) NULL,
[available] [smallint]  NULL DEFAULT (0))

ALTER TABLE [cms_operation] WITH NOCHECK ADD  CONSTRAINT [PK_cms_operation] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_operation] ON

INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 44,N'��������',N'link?view=list&type=friendlink',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 45,N'����->�����б�',N'catalog?view=list',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 46,N'ϵͳ->��վ����',N'config?id=1',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 47,N'ϵͳ->��վ���������޸�',N'config?id=2',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 48,N'ϵͳ->��վ�Ż�����',N'config?id=3',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 49,N'������־',N'system?view=errorlog',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 50,N'����->ҳ�����',N'archive?view=list&type=1',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 51,N'����->���ҳ��',N'archive?view=create&type=1',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 52,N'����->��Ϣ�б�',N'archive?view=list&type=2',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 53,N'����->������Ϣ',N'archive?view=create&type=2',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 54,N'����->ɾ������',N'app.axd?do=catalog:delete',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 55,N'����->�޸ķ���',N'app.axd?do=catalog:update',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 56,N'��Ա>��Ա�б�',N'user?view=member',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 57,N'��Ա>ɾ����Ա',N'app.axd?do=member:delete',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 58,N'ϵͳ�û�����',N'user?view=user',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 59,N'ɾ������',N'app.axd?do=archive:deletecomment',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 60,N'ͷ����������',N'link?view=list&type=headerlink',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 61,N'��վ��������',N'link?view=list&type=navigation',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 62,N'���������',N'link?view=create',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 63,N'�޸�����',N'link?view=edit',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 64,N'ɾ������',N'app.axd?do=link:delete',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 65,N'����ҳ��',N'archive?view=update&typeid=1',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 66,N'������Ϣ',N'archive?view=update&typeid=2',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 67,N'����ͼ����Ϣ',N'archive?view=update&typeid=3',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 68,N'���»�����Ϣ',N'archive?view=update&typeid=4',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 69,N'������Ƶ��Ϣ',N'archive?view=update&typeid=5',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 70,N'����ר����Ϣ',N'archive?view=update&typeid=6',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 71,N'ͼ����Ϣ�б�',N'archive?view=list&typeid=3',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 72,N'����Ϣ�б�',N'archive?view=list&typeid=4',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 73,N'��Ƶ��Ϣ�б�',N'archive?view=list&typeid=5',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 74,N'ר���б�',N'archive?view=list&typeid=6',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 75,N'����ͼ����Ϣ',N'archive?view=create&typeid=3',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 76,N'����������Ϣ',N'archive?view=create&typeid=4',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 77,N'������Ƶ��Ϣ',N'archive?view=create&typeid=5',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 78,N'������ר��',N'archive?view=create&typeid=6',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 79,N'�������',N'system?view=clearcache',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 80,N'�����б�',N'operation?view=list',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 81,N'�û������Ȩ������',N'operation?view=set',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 82,N'���������־',N'/app.axd?log:clearErrorLog',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 83,N'ɾ���ĵ�',N'archive:delete',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 84,N'ˢ���ĵ�����ʱ��',N'archive:refresh',1)
INSERT [cms_operation] ([id],[name],[path],[available]) VALUES ( 85,N'���ݲɼ�',N'/plugin/collection.ashx',1)

SET IDENTITY_INSERT [cms_operation] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_review]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_review]

CREATE TABLE [cms_review] (
[id] [nvarchar]  (765) NOT NULL,
[members] [ntext]  NOT NULL)

ALTER TABLE [cms_review] WITH NOCHECK ADD  CONSTRAINT [PK_cms_review] PRIMARY KEY  NONCLUSTERED ( [id] )
if exists (select * from sysobjects where id = OBJECT_ID('[cms_site]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_site]

CREATE TABLE [cms_site] (
[siteid] [int]  IDENTITY (1, 1)  NOT NULL,
[name] [nvarchar]  (150) NOT NULL,
[dirname] [nvarchar]  (150) NULL,
[domain] [nvarchar]  (150) NULL,
[location] [nvarchar]  (150) NULL,
[language] [int]  NOT NULL,
[tpl] [nvarchar]  (300) NULL,
[note] [nvarchar]  (600) NULL,
[seotitle] [nvarchar]  (600) NULL,
[seokeywords] [nvarchar]  (750) NULL,
[seodescription] [nvarchar]  (750) NULL,
[state] [int]  NOT NULL,
[protel] [nvarchar]  (150) NULL,
[prophone] [nvarchar]  (33) NULL,
[profax] [nvarchar]  (150) NULL,
[proaddress] [nvarchar]  (300) NULL,
[proemail] [nvarchar]  (300) NULL,
[proqq] [nvarchar]  (300) NULL,
[promsn] [nvarchar]  (300) NULL,
[pronotice] [nvarchar]  (750) NULL,
[proslogan] [nvarchar]  (750) NULL)

ALTER TABLE [cms_site] WITH NOCHECK ADD  CONSTRAINT [PK_cms_site] PRIMARY KEY  NONCLUSTERED ( [siteid] )
SET IDENTITY_INSERT [cms_site] ON

INSERT [cms_site] ([siteid],[name],[language],[tpl],[seotitle],[state],[pronotice]) VALUES ( 1,N'Ĭ��վ��',1,N'default',N'Ĭ��վ��-Speicial Cms .NET!',1,N'SPC.NET��һ���ƽ̨֧�ֶ�վ�����ASP.NET MVC�����ܹ������ݹ���ϵͳ!')

SET IDENTITY_INSERT [cms_site] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_table]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_table]

CREATE TABLE [cms_table] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[name] [nvarchar]  (150) NOT NULL,
[note] [nvarchar]  (1500) NULL,
[apiserver] [nvarchar]  (600) NULL,
[issystem] [bit]  NOT NULL DEFAULT (0),
[available] [bit]  NOT NULL DEFAULT (0))

ALTER TABLE [cms_table] WITH NOCHECK ADD  CONSTRAINT [PK_cms_table] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_table] ON


SET IDENTITY_INSERT [cms_table] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_table_column]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_table_column]

CREATE TABLE [cms_table_column] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[tableid] [int]  NOT NULL,
[name] [nvarchar]  (60) NOT NULL,
[note] [nvarchar]  (150) NULL,
[validformat] [nvarchar]  (600) NULL,
[orderindex] [int]  NOT NULL)

ALTER TABLE [cms_table_column] WITH NOCHECK ADD  CONSTRAINT [PK_cms_table_column] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_table_column] ON


SET IDENTITY_INSERT [cms_table_column] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_table_row]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_table_row]

CREATE TABLE [cms_table_row] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[tableid] [int]  NOT NULL,
[submittime] [datetime]  NULL)

ALTER TABLE [cms_table_row] WITH NOCHECK ADD  CONSTRAINT [PK_cms_table_row] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_table_row] ON


SET IDENTITY_INSERT [cms_table_row] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_table_rowdata]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_table_rowdata]

CREATE TABLE [cms_table_rowdata] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[rid] [int]  NOT NULL,
[cid] [int]  NOT NULL,
[value] [nvarchar]  (3000) NULL)

ALTER TABLE [cms_table_rowdata] WITH NOCHECK ADD  CONSTRAINT [PK_cms_table_rowdata] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_table_rowdata] ON


SET IDENTITY_INSERT [cms_table_rowdata] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_tplbind]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_tplbind]

CREATE TABLE [cms_tplbind] (
[id] [int]  IDENTITY (1, 1)  NOT NULL,
[bindid] [nvarchar]  (60) NOT NULL,
[bindtype] [int]  NOT NULL,
[tplpath] [nvarchar]  (600) NULL)

ALTER TABLE [cms_tplbind] WITH NOCHECK ADD  CONSTRAINT [PK_cms_tplbind] PRIMARY KEY  NONCLUSTERED ( [id] )
SET IDENTITY_INSERT [cms_tplbind] ON


SET IDENTITY_INSERT [cms_tplbind] OFF
if exists (select * from sysobjects where id = OBJECT_ID('[cms_usergroup]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_usergroup]

CREATE TABLE [cms_usergroup] (
[id] [int]  NOT NULL,
[name] [nvarchar]  (150) NULL,
[permissions] [nvarchar]  (765) NULL)

ALTER TABLE [cms_usergroup] WITH NOCHECK ADD  CONSTRAINT [PK_cms_usergroup] PRIMARY KEY  NONCLUSTERED ( [id] )
INSERT [cms_usergroup] ([id]) VALUES ( 0)
INSERT [cms_usergroup] ([id],[name],[permissions]) VALUES ( 1,N'��������Ա',N'1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42')
INSERT [cms_usergroup] ([id],[name],[permissions]) VALUES ( 2,N'����Ա',N'1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,40,41,42')
INSERT [cms_usergroup] ([id],[name],[permissions]) VALUES ( 3,N'�༭',N'1,2,3,4,5,6,10,11,12,13,14,15')
INSERT [cms_usergroup] ([id],[name],[permissions]) VALUES ( 4,N'��Ա',N'1,2,3,4,5,6')
INSERT [cms_usergroup] ([id],[name],[permissions]) VALUES ( 5,N'�ο�',N'3,4')
if exists (select * from sysobjects where id = OBJECT_ID('[cms_user]') and OBJECTPROPERTY(id, 'IsUserTable') = 1) 
DROP TABLE [cms_user]

CREATE TABLE [cms_user] (
[userid] [int]  IDENTITY (1, 1)  NOT NULL,
[siteid] [int]  NOT NULL DEFAULT (1),
[username] [nvarchar]  (150) NOT NULL,
[password] [nvarchar]  (150) NULL,
[name] [nvarchar]  (150) NULL,
[groupid] [int]  NULL,
[available] [smallint]  NULL,
[createdate] [datetime]  NULL,
[lastlogindate] [datetime]  NULL)

ALTER TABLE [cms_user] WITH NOCHECK ADD  CONSTRAINT [PK_cms_user] PRIMARY KEY  NONCLUSTERED ( [userid] )
SET IDENTITY_INSERT [cms_user] ON

INSERT [cms_user] ([userid],[siteid],[username],[password],[name],[groupid],[available],[createdate],[lastlogindate]) VALUES ( 1,0,N'admin',N'285c96f7702357c07f9b5daee2660e79',N'master',1,1,N'2013/12/18 0:00:00',N'2013/12/18 0:00:00')

SET IDENTITY_INSERT [cms_user] OFF
