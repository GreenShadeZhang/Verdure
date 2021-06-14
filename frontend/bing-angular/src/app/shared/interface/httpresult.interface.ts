import { StateCode } from '../enum/statecode.enum';

export interface Httpresult<T=any> {
    msg:string;
    result:T;
    code:StateCode;
}



