import { Controller } from '@nestjs/common';
import { ForosService } from './foros.service';

@Controller('foros')
export class ForosController {
  constructor(private readonly forosService: ForosService) {}
}
