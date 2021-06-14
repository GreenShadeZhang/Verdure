# MyBlogApi

## 这是一个我的blog api项目，前端使用的是angular写的，项目没有和api在同一个仓库，代码在下面的仓库里。

## [所用的前端项目地址](https://github.com/GreenShadeZhang/bing-angular).

# 跟微软的一个官方教程学习的，将项目分为三个，GreenShade.Blog.Domain主要为model相关的东西，GreenShade.Blog.DataAccess为数据库操作相关的东西。
# GreenShade.Blog.Api为api项目，jwt结合identity相关的api和用ef core对数据库进行操作，jwt授权接口，算是把从网友哪里抄的代码都是整合到一起了。
# 记得在GreenShade.Blog.DataAccess项目执行数据库迁移相关指令如下。

# 首次使用只需要把代码拉下来，然后将数据库链接上去就好了，然后把数据库表迁移下，就能使用了。数据库使用的是postgreSql，所以使用的时候不要安装错了。

## 创建迁移，此项目 已经创建迁移文件，所以可以不用执行这两个指令。
### add-migration InitialIdentityModel -Context AppIdentityDbContext -o Data/Identity  
### add-migration InitialBlogModel -Context BlogContext -o Data/Blog


## 更新数据库相关的表
### Update-Database InitialIdentityModel -Context AppIdentityDbContext
### Update-Database InitialBlogModel -Context BlogContext
