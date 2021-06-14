export interface Article {
    id: string;
    title: string;
    content: string;
    articleViews: number;
    articleCommentCount: number;
    articleDate: string;
    articleLikeCount: number;
    userId?: string;
    nickName?: string;
    avatar?: string;
    status: number;
    type: number;
    picUrl?:string;
    picInfo?:string;
  }
