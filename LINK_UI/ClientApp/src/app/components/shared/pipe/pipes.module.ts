import { NgModule } from '@angular/core';
import { NumberSuffixPipe } from './numberFormat.pipe';

import { DataFieldAccessPipe } from './dataFieldAccess.pipe';
import { DateUTCPipe } from './date-utc.pipe';

@NgModule({
    declarations: [
        NumberSuffixPipe,DataFieldAccessPipe, DateUTCPipe
    ],
    imports: [

    ],
    exports: [
        NumberSuffixPipe,DataFieldAccessPipe, DateUTCPipe
    ]
    
})
export class PipesModule {}