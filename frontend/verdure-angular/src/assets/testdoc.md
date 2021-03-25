## 为啥写这篇帖子呢？其实是因为翻微软的文档中心偶然翻到的，于是就出于好奇就试试了，看看用着怎么样。
<img src="https://img-blog.csdnimg.cn/20200227154030615.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MzUwMDQyNg==,size_16,color_FFFFFF,t_70" width="100%">

## 以前没注意图片，所以我今天发现的时候，显示EF Core3.1支持standard2.0，于是就想试试UWP用着会不会出问题，之前有的网友说2.1的standard目前UWP用不了。
## ef core具体用法 文档中心都有文档教程，首先是先创建一个 Standard2.0的项目，然后创建一个数据上下文，和一些模型。
## [dotnet控制台项目使用ef core的使用方法。](https://docs.microsoft.com/zh-cn/ef/core/get-started/?tabs=netcore-cli)
## 上面的帖子是官方的使用方法，下图是我的项目的，和大多数网友的没什么区别。
<img src="https://img-blog.csdnimg.cn/20200227154816226.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MzUwMDQyNg==,size_16,color_FFFFFF,t_70" width="100%">

## standard项目名字叫EFcore就是创建了model 类，和一个上下文，必装的包如下：
## Microsoft.EntityFrameworkCore.Sqlite 这个包，个人理解应该是主要用来连接数据库的，可以被UWP项目引用进而操作数据库。
## 然后我们需要一个dotnet core的控制台程序，用来创建迁移和执行迁移，要是用过asp.net core的大家都知道 其实用ef core很方便，一个项目就可以创建迁移和执行迁移。
<img src="https://img-blog.csdnimg.cn/20200227155616887.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MzUwMDQyNg==,size_16,color_FFFFFF,t_70" width="100%">

## 上图左边的图上我们是给文件路径传了一个路径，等会再说为什么给个路径，右边就是多装了一个库Microsoft.EntityFrameworkCore.Tools，这个主要是执行迁移指令的。下图就是在包管理控制台执行迁移指令
## Add-Migration InitialCreate 这个是创建数据库的表的骨架
## Update-Database 这就是把表给生成 文档中心都有写
<img src="https://img-blog.csdnimg.cn/20200227160018919.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MzUwMDQyNg==,size_16,color_FFFFFF,t_70" width="100%">

## 控制台程序设为启动程序，包管理那里默认项目为standard项目，上面做完把控制台程序执行完都很正常，说明基础已经做好。然后新建个uwp项目，用来测试uwp使用会不会出问题。
<img src="https://img-blog.csdnimg.cn/20200227160921632.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MzUwMDQyNg==,size_16,color_FFFFFF,t_70" width="100%">

## 图上是有报错，报错提示不能打开数据库文件，去git上找了原因是因为，在数据库上下文写的数据库文件名不是绝对路径，所以会导致找不到数据库，然后改了代码以后，发现正常了。

```csharp
 protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dbFile = await ApplicationData.Current.LocalFolder.TryGetItemAsync("blogging.db") as StorageFile;
            if (null == dbFile || SystemInformation.IsFirstRun)
            {
                // first time ... copy the .db file from assets to local  folder
                var localFolder = ApplicationData.Current.LocalFolder;
                var originalDbFileUri = new Uri("ms-appx:///Assets/blogging.db");
                var originalDbFile = await StorageFile.GetFileFromApplicationUriAsync(originalDbFileUri);
                if (null != originalDbFile)
                {

                    dbFile = await originalDbFile.CopyAsync(localFolder, "blogging.db", NameCollisionOption.ReplaceExisting);

                }
            }
            try
            {
                using (var db = new BloggingContext())
                {
                    //这里是传绝对路径给的数据上下文
                   db.DbFilePath = dbFile.Path;
                    db.Database.Migrate();
                    //// Create
                    db.Add(new Blog { Url = "http://blogs.msdn.com/adonet", TestUrl = "http://blogs.msdn.com/adonet" });
                    db.SaveChanges();
                    // Read
                    var blog = db.Blogs
                        .OrderBy(b => b.BlogId)
                        .ToList().Select(b => b.TestUrl);
                    SQLite.ItemsSource = blog;
                }
            }
            catch (Exception ex)
            {

            }

        }
```
## 具体好像就发现这一点和以前使用的区别，其他的好像都正常使用。下图的listview上半部分为什么没数据呢？是因为我测试了db.Database.Migrate();能不能正常迁移表新添的字段。所以上面的旧字段没值而已。

# **得出结论，对UWP支持还算良好，目测以后的大一统时代各种支持应该会更好，所以UWP应该算是没死掉的。**
<img src="https://img-blog.csdnimg.cn/20200227161501239.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl80MzUwMDQyNg==,size_16,color_FFFFFF,t_70" width="100%">

## 项目代码和参考的博客文章如下：有说的不对的请大家予以批评
## [博客项目代码地址](https://github.com/GreenShadeZhang/GreenShade.UWPDemo/tree/master/GreenShade.UWP.UseEFCore3.1)
## [参考博客地址](https://www.cnblogs.com/wpinfo/p/uwp_efcore_sqlite.html)
## [有启发的git讨论地址](https://github.com/xamarin/xamarin-android/issues/3819)

## 如果转载请注明出处（前提有人愿意转载😅）
