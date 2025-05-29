resource "aws_instance" "app_server" {
  ami                         = var.ami
  instance_type               = var.instance_type
  security_groups             = var.security_groups // for default VPC only. If you are creating Instances in a VPC, use vpc_security_group_ids instead.
  user_data                   = var.user_data
  user_data_replace_on_change = true
  key_name                    = var.key_name

  root_block_device {
    volume_size           = var.volume_size
    volume_type           = "gp3"
    delete_on_termination = true

    tags = {
      Name = "${var.app_name}-volume"
    }
  }

  tags = {
    Name = "${var.app_name}-ec2"
  }
}
