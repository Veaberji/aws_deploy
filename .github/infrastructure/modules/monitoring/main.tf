resource "aws_cloudwatch_metric_alarm" "cpu_utilization_high" {
  alarm_name          = "${var.app_name}-cpu-high"
  comparison_operator = "GreaterThanThreshold"
  evaluation_periods  = 2
  metric_name         = "CPUUtilization"
  namespace           = "AWS/EC2"
  period              = 120
  statistic           = "Average"
  threshold           = var.alarm_cpu_threshold
  alarm_description   = "Triggered when CPU exceeds threshold"
  actions_enabled     = true
  alarm_actions       = [var.sns_topic_arn]

  dimensions = {
    InstanceId = var.instance_id
  }

  tags = {
    Name = "${var.app_name}-cpu-alarm"
  }
}

resource "aws_cloudwatch_metric_alarm" "status_check_failed" {
  alarm_name          = "${var.app_name}-status-check-failed"
  comparison_operator = "GreaterThanThreshold"
  evaluation_periods  = 1
  metric_name         = "StatusCheckFailed"
  namespace           = "AWS/EC2"
  period              = 60
  statistic           = "Maximum"
  threshold           = 0
  alarm_description   = "Triggered when instance or system check fails"
  actions_enabled     = true
  alarm_actions       = [var.sns_topic_arn]

  dimensions = {
    InstanceId = var.instance_id
  }

  tags = {
    Name = "${var.app_name}-status-check-alarm"
  }
}
