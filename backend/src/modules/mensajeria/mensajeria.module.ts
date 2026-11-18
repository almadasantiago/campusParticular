import { Module } from '@nestjs/common';
import { MensajeriaService } from './mensajeria.service';
import { MensajeriaController } from './mensajeria.controller';

@Module({
  controllers: [MensajeriaController],
  providers: [MensajeriaService],
})
export class MensajeriaModule {}
