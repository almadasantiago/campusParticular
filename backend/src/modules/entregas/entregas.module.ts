import { Module } from '@nestjs/common';
import { EntregasService } from './entregas.service.js';
import { EntregasController } from './entregas.controller.js';

@Module({
  controllers: [EntregasController],
  providers: [EntregasService],
})
export class EntregasModule {}
