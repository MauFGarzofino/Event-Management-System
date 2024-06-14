﻿using EventMS.Application.DTOs;
using EventMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMS.Application.Port
{
    public interface ICreateEventUseCase
    {
        Event Execute(EventDto eventDto);
    }
}
