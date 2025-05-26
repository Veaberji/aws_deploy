resource "aws_sns_topic" "alarm_topic" {
  name = "${var.app_name}-alarm-topic"

  tags = {
    Name = "${var.app_name}-alarm-topic"
  }
}

resource "aws_sns_topic_subscription" "email_subscriptions" {
  for_each = toset(var.notification_emails)

  topic_arn = aws_sns_topic.alarm_topic.arn
  protocol  = "email"
  endpoint  = each.value
}
