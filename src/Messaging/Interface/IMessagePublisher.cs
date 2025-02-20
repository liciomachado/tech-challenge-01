﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Interface;

public interface IMessagePublisher<in T>
{
    Task PublishMessage(T message);
}
