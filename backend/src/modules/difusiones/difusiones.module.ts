import { Module } from '@nestjs/common';
import { DifusionesService } from './difusiones.service';
import { DifusionesController } from './difusiones.controller';

@Module({
  controllers: [DifusionesController],
  providers: [DifusionesService],
})
export class DifusionesModule {}
